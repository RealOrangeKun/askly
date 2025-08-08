// Configuration
const API_BASE_URL = 'http://localhost:5000/api';

// DOM Elements
const chatMessages = document.getElementById('chatMessages');
const messageInput = document.getElementById('messageInput');
const sendBtn = document.getElementById('sendBtn');
const loadingIndicator = document.getElementById('loadingIndicator');
const searchModeBtn = document.getElementById('searchModeBtn');
const createModeBtn = document.getElementById('createModeBtn');

// State
let currentMode = 'search'; // 'search' or 'create'
let isLoading = false;

// Utility Functions
function addPlaceholderMessage() {
    const messageDiv = document.createElement('div');
    messageDiv.className = 'flex mb-4 fade-in';
    messageDiv.id = 'placeholder-message';
    
    messageDiv.innerHTML = `
        <div class="message-bot max-w-2xl rounded-2xl px-4 py-3 shadow-sm">
            <div class="flex items-center mb-2">
                <div class="w-6 h-6 bg-gradient-to-br from-purple-600 to-blue-600 rounded-full flex items-center justify-center mr-2">
                    <i class="fas fa-robot text-white text-xs"></i>
                </div>
                <span class="text-sm font-medium text-gray-600">Askly Assistant</span>
                <span class="text-xs text-gray-400 ml-auto">${formatTimestamp()}</span>
            </div>
            <div class="flex items-center">
                <i class="fas fa-spinner loading text-purple-600 mr-2"></i>
                <span class="text-gray-600">${currentMode === 'search' ? 'Searching for similar questions...' : 'Processing your question...'}</span>
            </div>
        </div>
    `;
    
    chatMessages.appendChild(messageDiv);
    scrollToBottom();
    return messageDiv;
}

function replacePlaceholderMessage(placeholderElement, content, type = 'text') {
    if (!placeholderElement) return;
    
    let messageContent = '';
    
    if (type === 'results') {
        messageContent = createResultsMessage(content);
    } else if (type === 'success') {
        messageContent = createSuccessMessage(content);
    } else if (type === 'error') {
        messageContent = createErrorMessage(content);
    } else {
        messageContent = `<p class="text-gray-800">${escapeHtml(content)}</p>`;
    }
    
    placeholderElement.innerHTML = `
        <div class="message-bot max-w-2xl rounded-2xl px-4 py-3 shadow-sm">
            <div class="flex items-center mb-2">
                <div class="w-6 h-6 bg-gradient-to-br from-purple-600 to-blue-600 rounded-full flex items-center justify-center mr-2">
                    <i class="fas fa-robot text-white text-xs"></i>
                </div>
                <span class="text-sm font-medium text-gray-600">Askly Assistant</span>
                <span class="text-xs text-gray-400 ml-auto">${formatTimestamp()}</span>
            </div>
            ${messageContent}
        </div>
    `;
    
    scrollToBottom();
}

function scrollToBottom() {
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

function formatTimestamp() {
    return new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
}

// Message Functions
function addUserMessage(text) {
    const messageDiv = document.createElement('div');
    messageDiv.className = 'flex justify-end mb-4 fade-in';
    
    messageDiv.innerHTML = `
        <div class="message-user max-w-md rounded-2xl px-4 py-3 shadow-sm">
            <p class="text-white">${escapeHtml(text)}</p>
            <div class="text-xs text-gray-200 mt-1 text-right">${formatTimestamp()}</div>
        </div>
    `;
    
    chatMessages.appendChild(messageDiv);
    scrollToBottom();
}

function addBotMessage(content, type = 'text') {
    const messageDiv = document.createElement('div');
    messageDiv.className = 'flex mb-4 fade-in';
    
    let messageContent = '';
    
    if (type === 'results') {
        messageContent = createResultsMessage(content);
    } else if (type === 'success') {
        messageContent = createSuccessMessage(content);
    } else if (type === 'error') {
        messageContent = createErrorMessage(content);
    } else {
        messageContent = `<p class="text-gray-800">${escapeHtml(content)}</p>`;
    }
    
    messageDiv.innerHTML = `
        <div class="message-bot max-w-2xl rounded-2xl px-4 py-3 shadow-sm">
            <div class="flex items-center mb-2">
                <div class="w-6 h-6 bg-gradient-to-br from-purple-600 to-blue-600 rounded-full flex items-center justify-center mr-2">
                    <i class="fas fa-robot text-white text-xs"></i>
                </div>
                <span class="text-sm font-medium text-gray-600">Askly Assistant</span>
                <span class="text-xs text-gray-400 ml-auto">${formatTimestamp()}</span>
            </div>
            ${messageContent}
        </div>
    `;
    
    chatMessages.appendChild(messageDiv);
    scrollToBottom();
}

function createResultsMessage(questions) {
    if (!questions || questions.length === 0) {
        return `
            <div class="text-center py-4">
                <i class="fas fa-search text-3xl text-gray-300 mb-2"></i>
                <p class="text-gray-600 font-medium mb-2">No similar questions found</p>
                <p class="text-gray-500 text-sm mb-3">It looks like this is a unique question!</p>
                <div class="bg-blue-50 border border-blue-200 rounded-lg p-3">
                    <div class="flex items-start">
                        <i class="fas fa-lightbulb text-blue-500 mr-2 mt-0.5"></i>
                        <div>
                            <p class="text-blue-800 font-medium text-sm">Want to help others?</p>
                            <p class="text-blue-700 text-sm">Switch to <strong>Create Mode</strong> below to submit your question to our knowledge base!</p>
                        </div>
                    </div>
                </div>
            </div>
        `;
    }
    
    let html = `<p class="text-gray-800 mb-3">I found ${questions.length} similar question${questions.length > 1 ? 's' : ''} for you:</p>`;
    
    questions.forEach((question, index) => {
        html += `
            <div class="bg-gray-50 rounded-lg p-4 mb-3 border border-gray-200">
                <div class="mb-3">
                    <h4 class="font-semibold text-gray-900 mb-1 flex items-center">
                        <i class="fas fa-question-circle text-blue-500 mr-2 text-sm"></i>
                        Question ${index + 1}
                    </h4>
                    <p class="text-gray-700">${escapeHtml(question.questionText)}</p>
                </div>
                
                <div>
                    <h4 class="font-semibold text-gray-900 mb-1 flex items-center">
                        <i class="fas fa-lightbulb text-yellow-500 mr-2 text-sm"></i>
                        Answer
                    </h4>
                    <p class="text-gray-700">${escapeHtml(question.answerText)}</p>
                </div>
            </div>
        `;
    });
    
    // Add helpful guidance after showing results
    html += `
        <div class="bg-green-50 border border-green-200 rounded-lg p-3 mt-4">
            <div class="flex items-start">
                <i class="fas fa-question-circle text-green-500 mr-2 mt-0.5"></i>
                <div>
                    <p class="text-green-800 font-medium text-sm">Didn't find what you were looking for?</p>
                    <p class="text-green-700 text-sm">Switch to <strong>Create Mode</strong> below to submit your specific question!</p>
                </div>
            </div>
        </div>
    `;
    
    return html;
}

function createSuccessMessage(message) {
    return `
        <div class="bg-green-50 border border-green-200 rounded-lg p-3">
            <div class="flex items-center">
                <i class="fas fa-check-circle text-green-500 mr-2"></i>
                <p class="text-green-800 font-medium">${escapeHtml(message)}</p>
            </div>
        </div>
    `;
}

function createErrorMessage(message) {
    return `
        <div class="bg-red-50 border border-red-200 rounded-lg p-3">
            <div class="flex items-center">
                <i class="fas fa-exclamation-circle text-red-500 mr-2"></i>
                <p class="text-red-800 font-medium">${escapeHtml(message)}</p>
            </div>
        </div>
    `;
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Question validation function
function validateQuestion(text) {
    const trimmedText = text.trim();
    
    // Check minimum length
    if (trimmedText.length < 10) {
        return {
            isValid: false,
            message: "Please provide a more detailed question (at least 10 characters)."
        };
    }
    
    // Check maximum length
    if (trimmedText.length > 1000) {
        return {
            isValid: false,
            message: "Please keep your question under 500 characters."
        };
    }
    
    // Question word patterns
    const questionWords = [
        'what', 'how', 'why', 'when', 'where', 'who', 'which', 'whose',
        'can', 'could', 'would', 'should', 'will', 'do', 'does', 'did',
        'is', 'are', 'was', 'were', 'am', 'have', 'has', 'had'
    ];
    
    const lowerText = trimmedText.toLowerCase();
    const words = lowerText.split(/\s+/);
    
    // Check if it starts with a question word
    const startsWithQuestionWord = questionWords.some(qw => 
        lowerText.startsWith(qw + ' ') || lowerText.startsWith(qw + "'")
    );
    
    // Check if it ends with a question mark
    const endsWithQuestionMark = trimmedText.endsWith('?');
    
    // Check if it contains question words anywhere
    const containsQuestionWords = questionWords.some(qw => words.includes(qw));
    
    // Check for imperative patterns that might be questions
    const imperativePatterns = [
        /^(tell me|explain|describe|help)/i,
        /^(give me|show me|find)/i,
        /how to/i,
        /what is|what are|what's/i
    ];
    
    const hasImperativePattern = imperativePatterns.some(pattern => 
        pattern.test(trimmedText)
    );
    
    // Check if it's just a greeting or too generic
    const genericPhrases = [
        'hi', 'hello', 'hey', 'thanks', 'thank you', 'ok', 'okay', 'yes', 'no',
        'good', 'bad', 'test', 'testing', 'help', 'please'
    ];
    
    const isGeneric = genericPhrases.includes(lowerText) || 
                     words.length <= 2 && genericPhrases.some(phrase => lowerText.includes(phrase));
    
    if (isGeneric) {
        return {
            isValid: false,
            message: "Please ask a specific question. For example: 'How do I...?' or 'What is...?'"
        };
    }
    
    // Main validation logic
    if (endsWithQuestionMark || startsWithQuestionWord || containsQuestionWords || hasImperativePattern) {
        return { isValid: true };
    }
    
    // If none of the above, it might not be a question
    return {
        isValid: false,
        message: "This doesn't look like a question. Try starting with words like 'How', 'What', 'Why', or ending with '?'"
    };
}

// API Functions
async function makeRequest(url, options = {}) {
    try {
        const config = {
            headers: {
                'Content-Type': 'application/json',
                ...options.headers
            },
            ...options
        };

        const response = await fetch(url, config);
        const data = await response.json();

        if (!response.ok) {
            throw new Error(data.message || 'Request failed');
        }

        return data;
    } catch (error) {
        console.error('API request failed:', error);
        
        // Transform technical errors into user-friendly messages
        if (error.name === 'TypeError' && error.message.includes('fetch')) {
            throw new Error('Unable to connect to the service');
        }
        if (error.message.includes('Failed to fetch')) {
            throw new Error('Service is currently unavailable');
        }
        if (error.message.includes('NetworkError')) {
            throw new Error('Network connection error');
        }
        
        throw error;
    }
}

async function searchQuestions(questionText) {
    const encodedQuestion = encodeURIComponent(questionText);
    return await makeRequest(`${API_BASE_URL}/questions/ask?questionText=${encodedQuestion}`);
}

async function createQuestion(questionText) {
    return await makeRequest(`${API_BASE_URL}/questions`, {
        method: 'POST',
        body: JSON.stringify({ questionText })
    });
}

// Message Processing
async function processMessage(message) {
    const trimmedMessage = message.trim();
    
    if (!trimmedMessage) {
        addBotMessage('Please enter a question!', 'error');
        return;
    }
    
    // Validate the question
    const validation = validateQuestion(trimmedMessage);
    if (!validation.isValid) {
        addBotMessage(validation.message, 'error');
        return;
    }
    
    // Add placeholder message and disable input
    const placeholderElement = addPlaceholderMessage();
    isLoading = true;
    sendBtn.disabled = true;
    
    try {
        if (currentMode === 'search') {
            await handleSearch(trimmedMessage, placeholderElement);
        } else {
            await handleCreate(trimmedMessage, placeholderElement);
        }
    } catch (error) {
        replacePlaceholderMessage(placeholderElement, 'Something went wrong. Please try again in a moment.', 'error');
        console.error('Error processing message:', error);
    } finally {
        isLoading = false;
        sendBtn.disabled = false;
    }
}

async function handleSearch(questionText, placeholderElement) {
    try {
        const response = await searchQuestions(questionText);
        
        if (response.isSuccess) {
            replacePlaceholderMessage(placeholderElement, response.data, 'results');
        } else {
            replacePlaceholderMessage(placeholderElement, 'Sorry, I couldn\'t find any similar questions right now. Please try again later.', 'error');
        }
    } catch (error) {
        replacePlaceholderMessage(placeholderElement, 'I\'m having trouble searching for questions at the moment. Please try again later.', 'error');
    }
}

async function handleCreate(questionText, placeholderElement) {
    try {
        const response = await createQuestion(questionText);
        
        if (response.isSuccess) {
            replacePlaceholderMessage(placeholderElement, 'Great! Your question has been submitted successfully. It\'s now in our knowledge base and will be reviewed and answered by our team. Please check back later for the answer.', 'success');
        } else {
            replacePlaceholderMessage(placeholderElement, 'Sorry, I couldn\'t create your question right now. Please try again later.', 'error');
        }
    } catch (error) {
        replacePlaceholderMessage(placeholderElement, 'I\'m having trouble creating your question at the moment. Please try again later.', 'error');
    }
}

// Mode Management
function setMode(mode) {
    currentMode = mode;
    
    // Update button states
    searchModeBtn.classList.remove('active', 'bg-purple-100', 'text-purple-700');
    createModeBtn.classList.remove('active', 'bg-purple-100', 'text-purple-700');
    searchModeBtn.classList.add('bg-gray-100', 'text-gray-700');
    createModeBtn.classList.add('bg-gray-100', 'text-gray-700');
    
    if (mode === 'search') {
        searchModeBtn.classList.remove('bg-gray-100', 'text-gray-700');
        searchModeBtn.classList.add('active', 'bg-purple-100', 'text-purple-700');
        messageInput.placeholder = 'Ask your question here...';
        addBotMessage('I\'m now in Search Mode. Ask me any question and I\'ll find similar questions and answers from our knowledge base! 🔍');
    } else {
        createModeBtn.classList.remove('bg-gray-100', 'text-gray-700');
        createModeBtn.classList.add('active', 'bg-purple-100', 'text-purple-700');
        messageInput.placeholder = 'Enter your new question here...';
        addBotMessage('I\'m now in Create Mode. Share your question and I\'ll add it to our knowledge base for others to answer! ✨');
    }
}

// Auto-resize textarea
function autoResize() {
    messageInput.style.height = 'auto';
    messageInput.style.height = Math.min(messageInput.scrollHeight, 120) + 'px';
}

// Event Listeners
sendBtn.addEventListener('click', async () => {
    if (isLoading) return;
    
    const message = messageInput.value.trim();
    if (!message) return;
    
    addUserMessage(message);
    messageInput.value = '';
    autoResize();
    
    await processMessage(message);
});

messageInput.addEventListener('keypress', async (e) => {
    if (e.key === 'Enter' && !e.shiftKey) {
        e.preventDefault();
        if (!isLoading) {
            sendBtn.click();
        }
    }
});

messageInput.addEventListener('input', autoResize);

searchModeBtn.addEventListener('click', () => {
    if (currentMode !== 'search') {
        setMode('search');
    }
});

createModeBtn.addEventListener('click', () => {
    if (currentMode !== 'create') {
        setMode('create');
    }
});

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    messageInput.focus();
    autoResize();
});
