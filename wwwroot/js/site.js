// Municipal Services App JavaScript

// Gamification functionality for Report Issue page
function updateProgress() {
    const location = document.getElementById('Location');
    const category = document.getElementById('Category');
    const description = document.getElementById('Description');
    const file = document.getElementById('AttachedFile');
    
    let progress = 0;
    let encouragementMessage = '';
    
    if (location && location.value.trim() !== '') progress += 25;
    if (category && category.value !== '') progress += 25;
    if (description && description.value.trim() !== '') progress += 25;
    if (file && file.files.length > 0) progress += 25;
    
    const progressBar = document.getElementById('progressBar');
    const encouragementDiv = document.getElementById('encouragementMessage');
    
    if (progressBar) {
        progressBar.style.width = progress + '%';
        progressBar.setAttribute('aria-valuenow', progress);
        progressBar.textContent = progress + '%';
    }
    
    // Encouragement messages
    if (progress === 25) {
        encouragementMessage = 'Good start! Keep going!';
    } else if (progress === 50) {
        encouragementMessage = 'You\'re halfway there! Keep going!';
    } else if (progress === 75) {
        encouragementMessage = 'Almost there! Just one more field!';
    } else if (progress === 100) {
        encouragementMessage = 'Excellent! Great job completing the form!';
    }
    
    if (encouragementDiv) {
        encouragementDiv.textContent = encouragementMessage;
        encouragementDiv.style.display = encouragementMessage ? 'block' : 'none';
    }
}

// Add event listeners for form fields
document.addEventListener('DOMContentLoaded', function() {
    const formFields = ['Location', 'Category', 'Description', 'AttachedFile'];
    
    formFields.forEach(fieldId => {
        const field = document.getElementById(fieldId);
        if (field) {
            field.addEventListener('input', updateProgress);
            field.addEventListener('change', updateProgress);
        }
    });
    
    // Initialize progress
    updateProgress();
});

// Points system
function awardPoints() {
    let currentPoints = parseInt(sessionStorage.getItem('userPoints') || '0');
    currentPoints += 10;
    sessionStorage.setItem('userPoints', currentPoints.toString());
    
    // Show points alert
    const pointsAlert = document.createElement('div');
    pointsAlert.className = 'alert alert-success alert-dismissible fade show';
    pointsAlert.innerHTML = `
        <strong>Points Awarded!</strong> You earned 10 points! Total: ${currentPoints} points
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    const container = document.querySelector('.container');
    if (container) {
        container.insertBefore(pointsAlert, container.firstChild);
    }
    
    // Update points display
    updatePointsDisplay();
}

function updatePointsDisplay() {
    const currentPoints = parseInt(sessionStorage.getItem('userPoints') || '0');
    const pointsDisplay = document.getElementById('pointsDisplay');
    if (pointsDisplay) {
        pointsDisplay.textContent = `Total Points: ${currentPoints}`;
    }
}

// Clear form function
function clearForm() {
    document.getElementById('reportForm').reset();
    updateProgress();
}

// (removed unused search helpers)

// Initialize page-specific functionality
document.addEventListener('DOMContentLoaded', function() {
    // Initialize points display
    updatePointsDisplay();
    
    // Initialize search data structures
    if (!window.searchHistoryStack) {
        window.searchHistoryStack = [];
    }
    if (!window.recentSearchesQueue) {
        window.recentSearchesQueue = [];
    }
    if (!window.searchFrequency) {
        window.searchFrequency = {};
    }
});
