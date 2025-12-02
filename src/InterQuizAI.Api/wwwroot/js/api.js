const API_BASE = '/api';

const InterQuizAPI = {
    // Configuration
    async getConfigStatus() {
        const response = await fetch(`${API_BASE}/config/status`);
        return response.json();
    },

    async setMasterKey(masterKey) {
        const response = await fetch(`${API_BASE}/config/master-key`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ masterKey })
        });
        return response.json();
    },

    async verifyMasterKey(masterKey) {
        const response = await fetch(`${API_BASE}/config/verify-master-key`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ masterKey })
        });
        return response.json();
    },

    async setApiKey(apiKey, masterKey) {
        const response = await fetch(`${API_BASE}/config/api-key`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ apiKey, masterKey })
        });
        return response.json();
    },

    async getModel() {
        const response = await fetch(`${API_BASE}/config/model`);
        return response.json();
    },

    async setModel(modelName) {
        const response = await fetch(`${API_BASE}/config/model`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ modelName })
        });
        return response.json();
    },

    // Languages
    async getTechnologyTypes() {
        const response = await fetch(`${API_BASE}/languages/technology-types`);
        return response.json();
    },

    async getLanguages(type = null) {
        const url = type ? `${API_BASE}/languages/${type}` : `${API_BASE}/languages`;
        const response = await fetch(url);
        return response.json();
    },

    async addLanguage(name, displayName, technologyType) {
        const response = await fetch(`${API_BASE}/languages`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, displayName, technologyType })
        });
        return response.json();
    },

    async deleteLanguage(id) {
        const response = await fetch(`${API_BASE}/languages/${id}`, {
            method: 'DELETE'
        });
        return response.json();
    },

    async getCategories(type) {
        const response = await fetch(`${API_BASE}/languages/categories/${type}`);
        return response.json();
    },

    async getDifficultyLevels() {
        const response = await fetch(`${API_BASE}/languages/difficulty-levels`);
        return response.json();
    },

    // Quiz
    async generateQuiz(languageId, categoryId, difficultyLevelId, hint, masterKey) {
        const response = await fetch(`${API_BASE}/quiz/generate`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ languageId, categoryId, difficultyLevelId, hint, masterKey })
        });
        return response.json();
    },

    async generateOfflineQuiz(languageId, categoryId, difficultyLevelId, hint, masterKey) {
        const response = await fetch(`${API_BASE}/quiz/generate-offline`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ languageId, categoryId, difficultyLevelId, hint, masterKey })
        });
        return response.json();
    },

    async getSession(sessionGuid) {
        const response = await fetch(`${API_BASE}/quiz/${sessionGuid}`);
        return response.json();
    },

    async getQuestion(sessionGuid, questionNumber) {
        const response = await fetch(`${API_BASE}/quiz/${sessionGuid}/questions/${questionNumber}`);
        return response.json();
    },

    async submitAnswer(sessionGuid, questionId, selectedAnswerId) {
        const response = await fetch(`${API_BASE}/quiz/${sessionGuid}/answer`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ questionId, selectedAnswerId })
        });
        return response.ok;
    },

    async completeQuiz(sessionGuid) {
        const response = await fetch(`${API_BASE}/quiz/${sessionGuid}/complete`, {
            method: 'POST'
        });
        return response.json();
    },

    async getResults(sessionGuid) {
        const response = await fetch(`${API_BASE}/quiz/${sessionGuid}/results`);
        return response.json();
    },

    async getStoredQuestionsCount(languageId, categoryId, difficultyLevelId) {
        const response = await fetch(`${API_BASE}/quiz/stored-count?languageId=${languageId}&categoryId=${categoryId}&difficultyLevelId=${difficultyLevelId}`);
        return response.json();
    },

    // History
    async getHistory() {
        const response = await fetch(`${API_BASE}/history`);
        return response.json();
    },

    async getStats() {
        const response = await fetch(`${API_BASE}/history/stats`);
        return response.json();
    },

    async deleteHistory(sessionGuid) {
        const response = await fetch(`${API_BASE}/history/${sessionGuid}`, {
            method: 'DELETE'
        });
        return response.json();
    }
};

// Session storage for master key (only kept in memory during session)
const SessionManager = {
    getMasterKey() {
        return sessionStorage.getItem('masterKey');
    },

    setMasterKey(key) {
        sessionStorage.setItem('masterKey', key);
    },

    clearMasterKey() {
        sessionStorage.removeItem('masterKey');
    },

    isLoggedIn() {
        return !!this.getMasterKey();
    }
};
