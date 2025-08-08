using Askly.Api.Shared.Services;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Pgvector;

namespace Askly.Api.Infrastructure.Services;

public class EmbeddingService : IEmbeddingService, IDisposable
{
    private readonly InferenceSession _session;
    private readonly Dictionary<string, int> _vocab;
    private readonly int _maxLength = 512;

    public EmbeddingService()
    {
        string modelPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "all-MiniLM-L6-v2", "model.onnx");
        string vocabPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "all-MiniLM-L6-v2", "vocab.txt");

        if (!File.Exists(modelPath))
            throw new FileNotFoundException($"Model file not found at: {modelPath}");

        if (!File.Exists(vocabPath))
            throw new FileNotFoundException($"Vocabulary file not found at: {vocabPath}");

        try
        {
            _session = new InferenceSession(modelPath);



        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load ONNX model: {ex.Message}", ex);
        }
        _vocab = LoadVocabulary(vocabPath);
    }

    public async Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        return await Task.Run(() =>
        {
            // Simple tokenization (for now, we'll use a basic approach)
            List<string> tokens = SimpleTokenize(text);

            // Convert tokens to IDs
            int[] inputIds = tokens.Select(token => _vocab.GetValueOrDefault(token, _vocab.GetValueOrDefault("[UNK]", 100))).ToArray();

            // Pad or truncate to max length
            long[] inputIdsLong = new long[_maxLength];
            long[] attentionMaskLong = new long[_maxLength];
            long[] tokenTypeIdsLong = new long[_maxLength]; // Add token type IDs

            for (int i = 0; i < _maxLength; i++)
            {
                if (i < inputIds.Length)
                {
                    inputIdsLong[i] = inputIds[i];
                    attentionMaskLong[i] = 1;
                    tokenTypeIdsLong[i] = 0; // All tokens are from the first sentence
                }
                else
                {
                    inputIdsLong[i] = 0; // PAD token
                    attentionMaskLong[i] = 0;
                    tokenTypeIdsLong[i] = 0; // PAD token type
                }
            }

            // Create tensors with correct dimensions
            DenseTensor<long> inputIdsTensor = new(inputIdsLong, [1, _maxLength]);
            DenseTensor<long> attentionMaskTensor = new(attentionMaskLong, [1, _maxLength]);
            DenseTensor<long> tokenTypeIdsTensor = new(tokenTypeIdsLong, [1, _maxLength]); // Add token type tensor

            // Run inference with all required inputs
            List<NamedOnnxValue> inputs = new()
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIdsTensor),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMaskTensor),
            NamedOnnxValue.CreateFromTensor("token_type_ids", tokenTypeIdsTensor) // Add token type IDs
        };

            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = _session.Run(inputs);
            Tensor<float>? output = (results.FirstOrDefault(r => r.Name == "last_hidden_state")?.AsTensor<float>()) ?? throw new InvalidOperationException("Model did not return expected output");

            // Mean pooling
            int sequenceLength = output.Dimensions[1];
            int hiddenSize = output.Dimensions[2];
            float[] pooled = new float[hiddenSize];

            int validTokens = 0;
            for (int i = 0; i < sequenceLength; i++)
            {
                if (attentionMaskLong[i] == 1)
                {
                    validTokens++;
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        pooled[j] += output[0, i, j];
                    }
                }
            }

            // Average the pooled values
            if (validTokens > 0)
            {
                for (int i = 0; i < hiddenSize; i++)
                {
                    pooled[i] /= validTokens;
                }
            }

            // Normalize the embedding
            double norm = Math.Sqrt(pooled.Sum(x => x * x));
            if (norm > 0)
            {
                for (int i = 0; i < pooled.Length; i++)
                {
                    pooled[i] /= (float)norm;
                }
            }

            return new Vector(pooled);
        }, cancellationToken);
    }

    private static List<string> SimpleTokenize(string text)
    {
        // Basic tokenization - split on whitespace and punctuation
        var tokens = new List<string> { "[CLS]" };

        string[] words = text.ToLower()
            .Split([' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')', '[', ']', '{', '}'],
                   StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                tokens.Add(word);
            }
        }

        tokens.Add("[SEP]");
        return tokens;
    }

    private static Dictionary<string, int> LoadVocabulary(string vocabPath)
    {
        var vocab = new Dictionary<string, int>();
        string[] lines = File.ReadAllLines(vocabPath);

        for (int i = 0; i < lines.Length; i++)
        {
            vocab[lines[i]] = i;
        }

        return vocab;
    }

    public void Dispose()
    {
        _session?.Dispose();
        GC.SuppressFinalize(this);
    }
}
