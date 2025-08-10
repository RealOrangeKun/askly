// Admin Configuration
const API_BASE_URL = 'http://localhost:5000/api';

// State Management
let currentPage = 1;
let currentFilter = 'unanswered';
let currentPageSize = 10;
let totalPages = 1;
let currentQuestionId = null;

// Initialize Admin Panel
document.addEventListener('DOMContentLoaded', function() {
    checkApiKey();
    initializeEventListeners();
    loadQuestions();
});

// Check if API key exists and redirect if not
function checkApiKey() {
    const apiKey = localStorage.getItem('askly_api_key');
    if (!apiKey) {
        alert('No API key found. You will be redirected to the home page.');
        window.location.href = 'index.html';
        return;
    }
    updateApiKeyStatus();
}

// Update API key status in header
function updateApiKeyStatus() {
    const apiKey = localStorage.getItem('askly_api_key');
    const statusElement = document.getElementById('apiKeyStatus');
    if (apiKey) {
        statusElement.textContent = 'Key Set';
    } else {
        statusElement.textContent = 'No Key';
    }
}

// Initialize event listeners
function initializeEventListeners() {
    // API Key button
    document.getElementById('apiKeyBtn').addEventListener('click', showApiKeyModal);
    document.getElementById('saveApiKey').addEventListener('click', saveApiKey);
    document.getElementById('cancelApiKey').addEventListener('click', hideApiKeyModal);

    // Filters and controls
    document.getElementById('statusFilter').addEventListener('change', handleFilterChange);
    document.getElementById('pageSize').addEventListener('change', handlePageSizeChange);
    document.getElementById('refreshBtn').addEventListener('click', refreshQuestions);

    // Pagination
    document.getElementById('prevPage').addEventListener('click', () => changePage(currentPage - 1));
    document.getElementById('nextPage').addEventListener('click', () => changePage(currentPage + 1));

    // Answer modal
    document.getElementById('submitAnswer').addEventListener('click', submitAnswer);
    document.getElementById('cancelAnswer').addEventListener('click', hideAnswerModal);

    // Close modals on outside click
    document.getElementById('apiKeyModal').addEventListener('click', function(e) {
        if (e.target === this) hideApiKeyModal();
    });
    document.getElementById('answerModal').addEventListener('click', function(e) {
        if (e.target === this) hideAnswerModal();
    });
}

// API Key Management
function showApiKeyModal() {
    const modal = document.getElementById('apiKeyModal');
    const input = document.getElementById('apiKeyInput');
    input.value = localStorage.getItem('askly_api_key') || '';
    modal.classList.remove('hidden');
    input.focus();
}

function hideApiKeyModal() {
    document.getElementById('apiKeyModal').classList.add('hidden');
    document.getElementById('apiKeyInput').value = '';
}

function saveApiKey() {
    const apiKey = document.getElementById('apiKeyInput').value.trim();
    if (apiKey) {
        localStorage.setItem('askly_api_key', apiKey);
        showSuccessMessage('API key saved successfully');
        updateApiKeyStatus();
        hideApiKeyModal();
        refreshQuestions();
    } else {
        localStorage.removeItem('askly_api_key');
        showErrorMessage('API key removed');
        updateApiKeyStatus();
        hideApiKeyModal();
    }
}

// Questions Management
async function loadQuestions() {
    showLoading();
    
    try {
        const apiKey = localStorage.getItem('askly_api_key');
        if (!apiKey) {
            throw new Error('No API key found');
        }

        // Build query parameters - using correct API parameter names
        const params = new URLSearchParams({
            PageNumber: currentPage - 1, // API uses 0-based indexing
            PageSize: currentPageSize
        });

        if (currentFilter === 'answered') {
            params.append('Answered', 'true');
        } else if (currentFilter === 'unanswered') {
            params.append('Answered', 'false');
        }

        const response = await fetch(`${API_BASE_URL}/admin/questions?${params}`, {
            headers: {
                'x-api-key': apiKey, // Correct header name
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            if (response.status === 401) {
                throw new Error('Invalid API key');
            }
            throw new Error(`Server error: ${response.status}`);
        }

        const data = await response.json();
        
        // Check if the API response is successful
        if (!data.isSuccess) {
            throw new Error(data.message || 'API returned error');
        }
        
        // Extract questions from the data wrapper
        const questions = data.data || [];
        
        displayQuestions(questions);
        updatePagination(currentPage, currentPageSize, questions.length);
        updateStats({ questions, total: questions.length });
        
    } catch (error) {
        console.error('Error loading questions:', error);
        showErrorMessage(error.message || 'Failed to load questions');
        showEmptyState();
    } finally {
        hideLoading();
    }
}

function displayQuestions(questions) {
    const container = document.getElementById('questionsContainer');
    
    if (!questions || questions.length === 0) {
        showEmptyState();
        return;
    }

    const questionsHtml = questions.map(question => {
        // Determine status styling and actions based on questionStatus
        const getStatusConfig = (status) => {
            switch(status) {
                case 'Answered':
                    return {
                        badge: 'bg-green-100 text-green-800',
                        icon: 'fa-check-circle',
                        text: 'Answered',
                        buttonText: 'Edit Answer',
                        buttonClass: 'bg-blue-600 hover:bg-blue-700',
                        buttonIcon: 'fa-edit'
                    };
                case 'Closed':
                    return {
                        badge: 'bg-gray-100 text-gray-800',
                        icon: 'fa-lock',
                        text: 'Closed',
                        buttonText: 'Reopen',
                        buttonClass: 'bg-gray-600 hover:bg-gray-700',
                        buttonIcon: 'fa-unlock'
                    };
                case 'Unanswered':
                default:
                    return {
                        badge: 'bg-orange-100 text-orange-800',
                        icon: 'fa-clock',
                        text: 'Unanswered',
                        buttonText: 'Answer',
                        buttonClass: 'bg-green-600 hover:bg-green-700',
                        buttonIcon: 'fa-reply'
                    };
            }
        };

        const statusConfig = getStatusConfig(question.questionStatus);
        
        return `
        <div class="border-b border-gray-200 last:border-b-0">
            <div class="p-6">
                <div class="flex items-start justify-between">
                    <div class="flex-1">
                        <div class="flex items-center mb-2">
                            <span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${statusConfig.badge}">
                                <i class="fas ${statusConfig.icon} mr-1"></i>
                                ${statusConfig.text}
                            </span>
                            <span class="ml-3 text-sm text-gray-500">
                                <i class="fas fa-fingerprint mr-1"></i>
                                ID: ${question.questionId}
                            </span>
                        </div>
                        <h3 class="text-lg font-medium text-gray-900 mb-2">${escapeHtml(question.questionText)}</h3>
                        ${question.questionStatus === 'Answered' ? `
                            <div class="bg-green-50 border border-green-200 rounded-lg p-3 mt-3">
                                <div class="flex items-center mb-1">
                                    <i class="fas fa-reply text-green-600 mr-2"></i>
                                    <span class="text-sm font-medium text-green-800">This question has been answered</span>
                                </div>
                                <p class="text-green-700 text-sm">Note: Answer text not available in current API response</p>
                            </div>
                        ` : ''}
                    </div>
                    <div class="ml-4 flex-shrink-0">
                        ${question.questionStatus !== 'Closed' ? `
                            <button 
                                onclick="showAnswerModal('${question.questionId}', '${escapeHtml(question.questionText).replace(/'/g, '\\\'')}')"
                                class="${statusConfig.buttonClass} text-white px-4 py-2 rounded-lg transition-colors text-sm"
                            >
                                <i class="fas ${statusConfig.buttonIcon} mr-1"></i>
                                ${statusConfig.buttonText}
                            </button>
                        ` : `
                            <span class="text-gray-500 text-sm">
                                <i class="fas fa-lock mr-1"></i>
                                Question Closed
                            </span>
                        `}
                    </div>
                </div>
            </div>
        </div>
        `;
    }).join('');

    container.innerHTML = questionsHtml;
    container.classList.remove('hidden');
    document.getElementById('emptyState').classList.add('hidden');
}

function updateStats(data) {
    const questions = data.questions || [];
    const total = questions.length;
    
    // Count questions by status
    const answered = questions.filter(q => q.questionStatus === 'Answered').length;
    const unanswered = questions.filter(q => q.questionStatus === 'Unanswered').length;
    const closed = questions.filter(q => q.questionStatus === 'Closed').length;
    
    // Update the display
    document.getElementById('totalQuestions').textContent = `${total} (current page)`;
    document.getElementById('answeredQuestions').textContent = answered;
    document.getElementById('unansweredQuestions').textContent = unanswered;
    
    // We could add a closed count if needed
    // For now, closed questions are included in the total
}

function updatePagination(page, perPage, currentCount) {
    currentPage = page;
    
    // Since API doesn't return total count, we'll use simple next/prev logic
    const start = ((page - 1) * perPage) + 1;
    const end = start + currentCount - 1;
    
    document.getElementById('pageInfo').textContent = `${start}-${end} (page ${page})`;
    document.getElementById('prevPage').disabled = page <= 1;
    // Disable next if we got less than requested (indicates last page)
    document.getElementById('nextPage').disabled = currentCount < perPage;
    
    if (currentCount > 0) {
        document.getElementById('pagination').classList.remove('hidden');
    } else {
        document.getElementById('pagination').classList.add('hidden');
    }
}

// Answer Management
function showAnswerModal(questionId, questionText) {
    currentQuestionId = questionId;
    document.getElementById('questionToAnswer').textContent = questionText;
    document.getElementById('answerInput').value = '';
    document.getElementById('answerModal').classList.remove('hidden');
    document.getElementById('answerInput').focus();
}

function hideAnswerModal() {
    document.getElementById('answerModal').classList.add('hidden');
    currentQuestionId = null;
    document.getElementById('answerInput').value = '';
}

async function submitAnswer() {
    const answer = document.getElementById('answerInput').value.trim();
    
    if (!answer) {
        showErrorMessage('Please provide an answer');
        return;
    }

    if (!currentQuestionId) {
        showErrorMessage('No question selected');
        return;
    }

    try {
        const apiKey = localStorage.getItem('askly_api_key');
        if (!apiKey) {
            throw new Error('No API key found');
        }

        const response = await fetch(`${API_BASE_URL}/admin/questions/${currentQuestionId}`, {
            method: 'PATCH',
            headers: {
                'x-api-key': apiKey, // Correct header name
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ answerText: answer }) // Correct property name
        });

        if (!response.ok) {
            if (response.status === 401) {
                throw new Error('Invalid API key');
            }
            throw new Error(`Server error: ${response.status}`);
        }

        showSuccessMessage('Answer submitted successfully');
        hideAnswerModal();
        refreshQuestions();
        
    } catch (error) {
        console.error('Error submitting answer:', error);
        showErrorMessage(error.message || 'Failed to submit answer');
    }
}

// Event Handlers
function handleFilterChange() {
    currentFilter = document.getElementById('statusFilter').value;
    currentPage = 1;
    loadQuestions();
}

function handlePageSizeChange() {
    currentPageSize = parseInt(document.getElementById('pageSize').value);
    currentPage = 1;
    loadQuestions();
}

function refreshQuestions() {
    loadQuestions();
}

function changePage(page) {
    if (page >= 1) {
        currentPage = page;
        loadQuestions();
    }
}

// UI Helper Functions
function showLoading() {
    document.getElementById('loadingQuestions').classList.remove('hidden');
    document.getElementById('questionsContainer').classList.add('hidden');
    document.getElementById('emptyState').classList.add('hidden');
}

function hideLoading() {
    document.getElementById('loadingQuestions').classList.add('hidden');
}

function showEmptyState() {
    document.getElementById('emptyState').classList.remove('hidden');
    document.getElementById('questionsContainer').classList.add('hidden');
    document.getElementById('pagination').classList.add('hidden');
}

function showSuccessMessage(message) {
    const element = document.getElementById('successMessage');
    document.getElementById('successText').textContent = message;
    element.classList.remove('hidden');
    setTimeout(() => element.classList.add('hidden'), 3000);
}

function showErrorMessage(message) {
    const element = document.getElementById('errorMessage');
    document.getElementById('errorText').textContent = message;
    element.classList.remove('hidden');
    setTimeout(() => element.classList.add('hidden'), 5000);
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
