// Internationalization (i18n) system
const translations = {
    pl: {
        // Navigation
        nav: {
            start: 'Start',
            history: 'Historia',
            languages: 'Języki',
            settings: 'Ustawienia'
        },
        // Index page
        index: {
            title: 'Przygotuj się do rozmowy technicznej',
            subtitle: 'Quizy generowane przez AI z weryfikowanymi odpowiedziami',
            configAlert: 'Najpierw skonfiguruj aplikację w',
            configAlertLink: 'Ustawieniach',
            login: 'Zaloguj się',
            masterKey: 'Master Key',
            masterKeyPlaceholder: 'Wprowadź master key',
            loginBtn: 'Zaloguj',
            startQuiz: 'Rozpocznij Quiz',
            techType: 'Typ technologii',
            techTypePlaceholder: 'Wybierz typ...',
            language: 'Język / Framework',
            languagePlaceholder: 'Najpierw wybierz typ...',
            languageSelect: 'Wybierz język...',
            category: 'Kategoria pytań',
            categoryPlaceholder: 'Najpierw wybierz typ...',
            categorySelect: 'Wybierz kategorię...',
            difficulty: 'Poziom trudności',
            difficultyPlaceholder: 'Wybierz poziom...',
            quizLanguage: 'Język quizu',
            hint: 'Dodatkowe wskazówki dla AI (opcjonalne)',
            hintPlaceholder: 'np. TimescaleDB, duże zbiory danych, Pinia, xUnit',
            hintHelp: 'Pomaga AI skupić się na konkretnych technologiach, bibliotekach lub zagadnieniach',
            offlineAvailable: 'Dostępny tryb offline',
            storedQuestions: 'zapisanych pytań',
            generateBtn: 'Generuj Quiz (AI)',
            offlineBtn: 'Quiz Offline',
            loading: 'Generowanie quizu przez AI...',
            loadingTime: 'To może potrwać do 60 sekund',
            studyPlan: 'Twój plan nauki (.NET Mid + Vue3)',
            studyPlanTotal: 'Razem: 12 sesji = ~240 pytań',
            invalidMasterKey: 'Nieprawidłowy master key',
            generateError: 'Błąd podczas generowania quizu: '
        },
        // Quiz page
        quiz: {
            question: 'Pytanie',
            of: 'z',
            answers: 'odpowiedzi',
            prev: 'Poprzednie',
            skip: 'Pomiń',
            next: 'Następne',
            finish: 'Zakończ Quiz i zobacz wyniki',
            confirmFinish: 'Czy na pewno chcesz zakończyć quiz? Nie będzie można zmienić odpowiedzi.'
        },
        // Results page
        results: {
            title: 'Quiz Zakończony!',
            correct: 'poprawnych odpowiedzi',
            reviewTitle: 'Przegląd odpowiedzi',
            newQuiz: 'Nowy Quiz',
            history: 'Historia',
            explanation: 'Wyjaśnienie',
            source: 'Źródło',
            statusCorrect: 'Poprawne',
            statusIncorrect: 'Błędne',
            statusSkipped: 'Pominięte'
        },
        // History page
        history: {
            title: 'Historia Quizów',
            stats: 'Statystyki',
            completedQuizzes: 'Ukończonych quizów',
            avgScore: 'Średni wynik',
            totalQuestions: 'Odpowiedzianych pytań',
            quizList: 'Lista quizów',
            date: 'Data',
            language: 'Język',
            category: 'Kategoria',
            level: 'Poziom',
            score: 'Wynik',
            type: 'Typ',
            actions: 'Akcje',
            view: 'Zobacz',
            repeat: 'Powtórz',
            delete: 'Usuń',
            empty: 'Brak historii quizów',
            startFirst: 'Rozpocznij pierwszy quiz',
            confirmDelete: 'Czy na pewno chcesz usunąć ten quiz z historii?',
            creating: 'Tworzenie quizu...',
            repeatError: 'Błąd podczas tworzenia quizu: '
        },
        // Config page
        config: {
            title: 'Ustawienia',
            apiConfig: 'Konfiguracja API',
            notConfigured: 'Nie skonfigurowano',
            configured: 'Skonfigurowano',
            masterKeySection: 'Master Key',
            masterKeyInfo: 'Hasło do szyfrowania klucza API (min. 16 znaków)',
            masterKeyPlaceholder: 'Wprowadź master key (min. 16 znaków)',
            masterKeyBtn: 'Ustaw Master Key',
            masterKeyChange: 'Zmień Master Key',
            apiKeySection: 'Klucz API OpenAI',
            apiKeyInfo: 'Klucz będzie zaszyfrowany master key\'em',
            apiKeyPlaceholder: 'sk-...',
            apiKeyBtn: 'Zapisz klucz API',
            modelSection: 'Model OpenAI',
            modelInfo: 'Wpisz nazwę modelu',
            modelPlaceholder: 'np. gpt-4o, gpt-4.1-2025-04-14',
            modelBtn: 'Zapisz model',
            currentModel: 'Aktualny model',
            logout: 'Wyloguj',
            masterKeySet: 'Master key został ustawiony',
            masterKeyError: 'Błąd podczas ustawiania master key',
            apiKeySaved: 'Klucz API został zapisany',
            apiKeyError: 'Błąd podczas zapisywania klucza API',
            modelSaved: 'Model został zapisany',
            modelError: 'Błąd podczas zapisywania modelu',
            minLength: 'Master key musi mieć minimum 16 znaków'
        },
        // Languages page
        languages: {
            title: 'Języki programowania',
            backendTitle: 'Backend',
            frontendTitle: 'Frontend',
            addNew: 'Dodaj nowy język',
            name: 'Nazwa (kod)',
            namePlaceholder: 'np. kotlin',
            displayName: 'Nazwa wyświetlana',
            displayNamePlaceholder: 'np. Kotlin',
            type: 'Typ',
            backend: 'Backend',
            frontend: 'Frontend',
            addBtn: 'Dodaj język',
            deleteConfirm: 'Czy na pewno chcesz usunąć ten język?',
            added: 'Język został dodany',
            addError: 'Błąd podczas dodawania języka',
            deleted: 'Język został usunięty',
            deleteError: 'Błąd podczas usuwania języka'
        },
        // Table headers
        table: {
            type: 'Typ',
            language: 'Język',
            category: 'Kategoria',
            sessions: 'Sesje',
            hint: 'Hint'
        }
    },
    en: {
        // Navigation
        nav: {
            start: 'Start',
            history: 'History',
            languages: 'Languages',
            settings: 'Settings'
        },
        // Index page
        index: {
            title: 'Prepare for your technical interview',
            subtitle: 'AI-generated quizzes with verified answers',
            configAlert: 'First configure the application in',
            configAlertLink: 'Settings',
            login: 'Log in',
            masterKey: 'Master Key',
            masterKeyPlaceholder: 'Enter master key',
            loginBtn: 'Log in',
            startQuiz: 'Start Quiz',
            techType: 'Technology type',
            techTypePlaceholder: 'Select type...',
            language: 'Language / Framework',
            languagePlaceholder: 'First select type...',
            languageSelect: 'Select language...',
            category: 'Question category',
            categoryPlaceholder: 'First select type...',
            categorySelect: 'Select category...',
            difficulty: 'Difficulty level',
            difficultyPlaceholder: 'Select level...',
            quizLanguage: 'Quiz language',
            hint: 'Additional hints for AI (optional)',
            hintPlaceholder: 'e.g. TimescaleDB, large datasets, Pinia, xUnit',
            hintHelp: 'Helps AI focus on specific technologies, libraries or topics',
            offlineAvailable: 'Offline mode available',
            storedQuestions: 'stored questions',
            generateBtn: 'Generate Quiz (AI)',
            offlineBtn: 'Offline Quiz',
            loading: 'Generating quiz with AI...',
            loadingTime: 'This may take up to 60 seconds',
            studyPlan: 'Your study plan (.NET Mid + Vue3)',
            studyPlanTotal: 'Total: 12 sessions = ~240 questions',
            invalidMasterKey: 'Invalid master key',
            generateError: 'Error generating quiz: '
        },
        // Quiz page
        quiz: {
            question: 'Question',
            of: 'of',
            answers: 'answers',
            prev: 'Previous',
            skip: 'Skip',
            next: 'Next',
            finish: 'Finish Quiz and see results',
            confirmFinish: 'Are you sure you want to finish the quiz? You won\'t be able to change answers.'
        },
        // Results page
        results: {
            title: 'Quiz Completed!',
            correct: 'correct answers',
            reviewTitle: 'Answers review',
            newQuiz: 'New Quiz',
            history: 'History',
            explanation: 'Explanation',
            source: 'Source',
            statusCorrect: 'Correct',
            statusIncorrect: 'Incorrect',
            statusSkipped: 'Skipped'
        },
        // History page
        history: {
            title: 'Quiz History',
            stats: 'Statistics',
            completedQuizzes: 'Completed quizzes',
            avgScore: 'Average score',
            totalQuestions: 'Questions answered',
            quizList: 'Quiz list',
            date: 'Date',
            language: 'Language',
            category: 'Category',
            level: 'Level',
            score: 'Score',
            type: 'Type',
            actions: 'Actions',
            view: 'View',
            repeat: 'Repeat',
            delete: 'Delete',
            empty: 'No quiz history',
            startFirst: 'Start your first quiz',
            confirmDelete: 'Are you sure you want to delete this quiz from history?',
            creating: 'Creating quiz...',
            repeatError: 'Error creating quiz: '
        },
        // Config page
        config: {
            title: 'Settings',
            apiConfig: 'API Configuration',
            notConfigured: 'Not configured',
            configured: 'Configured',
            masterKeySection: 'Master Key',
            masterKeyInfo: 'Password for encrypting API key (min. 16 characters)',
            masterKeyPlaceholder: 'Enter master key (min. 16 characters)',
            masterKeyBtn: 'Set Master Key',
            masterKeyChange: 'Change Master Key',
            apiKeySection: 'OpenAI API Key',
            apiKeyInfo: 'Key will be encrypted with master key',
            apiKeyPlaceholder: 'sk-...',
            apiKeyBtn: 'Save API key',
            modelSection: 'OpenAI Model',
            modelInfo: 'Enter model name',
            modelPlaceholder: 'e.g. gpt-4o, gpt-4.1-2025-04-14',
            modelBtn: 'Save model',
            currentModel: 'Current model',
            logout: 'Log out',
            masterKeySet: 'Master key has been set',
            masterKeyError: 'Error setting master key',
            apiKeySaved: 'API key has been saved',
            apiKeyError: 'Error saving API key',
            modelSaved: 'Model has been saved',
            modelError: 'Error saving model',
            minLength: 'Master key must be at least 16 characters'
        },
        // Languages page
        languages: {
            title: 'Programming Languages',
            backendTitle: 'Backend',
            frontendTitle: 'Frontend',
            addNew: 'Add new language',
            name: 'Name (code)',
            namePlaceholder: 'e.g. kotlin',
            displayName: 'Display name',
            displayNamePlaceholder: 'e.g. Kotlin',
            type: 'Type',
            backend: 'Backend',
            frontend: 'Frontend',
            addBtn: 'Add language',
            deleteConfirm: 'Are you sure you want to delete this language?',
            added: 'Language has been added',
            addError: 'Error adding language',
            deleted: 'Language has been deleted',
            deleteError: 'Error deleting language'
        },
        // Table headers
        table: {
            type: 'Type',
            language: 'Language',
            category: 'Category',
            sessions: 'Sessions',
            hint: 'Hint'
        }
    }
};

// i18n Manager
const I18n = {
    currentLang: 'pl',

    init() {
        const savedLang = localStorage.getItem('appLanguage') || 'pl';
        this.setLanguage(savedLang, false);
        this.updateLanguageSwitcher();
    },

    setLanguage(lang, save = true) {
        this.currentLang = lang;
        document.documentElement.lang = lang;
        if (save) {
            localStorage.setItem('appLanguage', lang);
        }
        this.updatePage();
        this.updateLanguageSwitcher();
    },

    t(key) {
        const keys = key.split('.');
        let value = translations[this.currentLang];
        for (const k of keys) {
            if (value && value[k] !== undefined) {
                value = value[k];
            } else {
                console.warn(`Translation missing: ${key}`);
                return key;
            }
        }
        return value;
    },

    updatePage() {
        // Update all elements with data-i18n attribute
        document.querySelectorAll('[data-i18n]').forEach(el => {
            const key = el.getAttribute('data-i18n');
            el.textContent = this.t(key);
        });

        // Update all elements with data-i18n-placeholder attribute
        document.querySelectorAll('[data-i18n-placeholder]').forEach(el => {
            const key = el.getAttribute('data-i18n-placeholder');
            el.placeholder = this.t(key);
        });

        // Update all elements with data-i18n-title attribute
        document.querySelectorAll('[data-i18n-title]').forEach(el => {
            const key = el.getAttribute('data-i18n-title');
            el.title = this.t(key);
        });

        // Update page title
        const titleEl = document.querySelector('title[data-i18n]');
        if (titleEl) {
            const key = titleEl.getAttribute('data-i18n');
            document.title = this.t(key) + ' - InterQuizAI';
        }
    },

    updateLanguageSwitcher() {
        const switcher = document.getElementById('lang-switcher');
        if (switcher) {
            switcher.value = this.currentLang;
        }
    },

    toggle() {
        this.setLanguage(this.currentLang === 'pl' ? 'en' : 'pl');
    }
};

// Auto-init when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    I18n.init();
});
