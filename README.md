# InterQuizAI

**[PL](#pl---polski) | [EN](#en---english)**

---

## PL - Polski

### O projekcie

**InterQuizAI** to aplikacja do przygotowania się do rozmów kwalifikacyjnych na stanowiska programistyczne. Generuje quizy techniczne przy użyciu AI z **weryfikacją odpowiedzi przez przeszukiwanie internetu**.

### Kluczowe funkcje

- **Generowanie quizów przez AI** - 20 pytań z 5 odpowiedziami każde
- **Weryfikacja danych z internetu** - każda odpowiedź jest weryfikowana przez web search
- **Źródła odpowiedzi** - do każdego pytania dołączane są linki do źródeł
- **Szczegółowe wyjaśnienia** - po zakończeniu quizu otrzymujesz wyjaśnienia do każdej odpowiedzi
- **Tryb offline** - możliwość rozwiązywania quizów z zapisanych wcześniej pytań (bez kosztów API)
- **Powtarzanie quizów** - możliwość powtórzenia dowolnego ukończonego quizu
- **Historia i statystyki** - śledzenie postępów i wyników
- **Wielojęzyczny interfejs** - polski i angielski (PL/EN)
- **Własne języki programowania** - możliwość dodawania własnych technologii

### Wspierane technologie

**Backend:** C#/.NET, Java/Spring, Python, Node.js, Go, Rust, PHP, Ruby

**Frontend:** Vue 3, React, Angular, Svelte, TypeScript, JavaScript

### Kategorie pytań

- Fundamenty języka
- Architektura i wzorce projektowe
- Bazy danych
- API i komunikacja
- Jakość i testy
- Bezpieczeństwo
- DevOps i narzędzia
- State Management (frontend)
- Routing (frontend)
- Performance i optymalizacja

### Poziomy trudności

| Poziom | Doświadczenie |
|--------|---------------|
| Junior | 0-2 lata |
| Mid | 2-5 lat |
| Senior | 5-8 lat |
| Tech Lead | 8+ lat |
| Architect | 10+ lat |

### Koszty i model AI

> **Uwaga:** Aplikacja wykorzystuje **OpenAI Responses API z funkcją Web Search**, co generuje wyższe koszty niż standardowe zapytania do API.

**Domyślny model:** `gpt-4o`

Koszty są wyższe ze względu na:
- Przeszukiwanie internetu dla każdego pytania (weryfikacja odpowiedzi)
- Generowanie źródeł i cytatów
- Szczegółowe wyjaśnienia z linkami

Możesz zmienić model w ustawieniach aplikacji na dowolny wspierany przez OpenAI (np. `gpt-4o-mini` dla niższych kosztów).

**Tryb offline** pozwala rozwiązywać quizy z wcześniej wygenerowanych pytań bez dodatkowych kosztów API.

### Stack technologiczny

- **.NET 9** - ASP.NET Core Minimal API
- **SQLite** - lokalna baza danych
- **HTML/CSS/JS** - frontend bez frameworków
- **OpenAI Responses API** - generowanie quizów z web search

### Uruchomienie

```bash
cd src/InterQuizAI.Api
dotnet run
```

Aplikacja uruchomi się na `http://localhost:5000`

### Konfiguracja

1. Otwórz aplikację w przeglądarce
2. Przejdź do **Ustawienia**
3. Ustaw **Master Key** (hasło do szyfrowania, min. 16 znaków)
4. Wprowadź **OpenAI API Key**
5. Opcjonalnie zmień model AI

---

<img width="1879" height="828" alt="image" src="https://github.com/user-attachments/assets/e8f08f19-1e29-4c88-97bf-261566294b22" />
<img width="914" height="706" alt="image" src="https://github.com/user-attachments/assets/76e0d47b-30b1-4320-96a8-ce7e67f963ae" />
<img width="1881" height="890" alt="image" src="https://github.com/user-attachments/assets/259a36a8-6fdb-43cd-9673-d7a9e9995297" />
<img width="1900" height="860" alt="image" src="https://github.com/user-attachments/assets/13b3be6a-9956-490d-9028-0123e1e7d87a" />
<img width="1911" height="852" alt="image" src="https://github.com/user-attachments/assets/834519a0-6968-4646-9dc4-9ae68bf339e7" />


## EN - English

### About

**InterQuizAI** is an application for preparing for programming job interviews. It generates technical quizzes using AI with **answer verification through internet search**.

### Key Features

- **AI-powered quiz generation** - 20 questions with 5 answers each
- **Internet data verification** - each answer is verified through web search
- **Answer sources** - links to sources are attached to each question
- **Detailed explanations** - after completing the quiz, you receive explanations for each answer
- **Offline mode** - ability to solve quizzes from previously saved questions (no API costs)
- **Quiz repeat** - ability to repeat any completed quiz
- **History and statistics** - track your progress and results
- **Multilingual interface** - Polish and English (PL/EN)
- **Custom programming languages** - ability to add your own technologies

### Supported Technologies

**Backend:** C#/.NET, Java/Spring, Python, Node.js, Go, Rust, PHP, Ruby

**Frontend:** Vue 3, React, Angular, Svelte, TypeScript, JavaScript

### Question Categories

- Language fundamentals
- Architecture and design patterns
- Databases
- API and communication
- Quality and testing
- Security
- DevOps and tools
- State Management (frontend)
- Routing (frontend)
- Performance and optimization

### Difficulty Levels

| Level | Experience |
|-------|------------|
| Junior | 0-2 years |
| Mid | 2-5 years |
| Senior | 5-8 years |
| Tech Lead | 8+ years |
| Architect | 10+ years |

### Costs and AI Model

> **Note:** The application uses **OpenAI Responses API with Web Search feature**, which generates higher costs than standard API queries.

**Default model:** `gpt-4o`

Costs are higher due to:
- Internet search for each question (answer verification)
- Source and citation generation
- Detailed explanations with links

You can change the model in application settings to any supported by OpenAI (e.g., `gpt-4o-mini` for lower costs).

**Offline mode** allows you to solve quizzes from previously generated questions without additional API costs.

### Tech Stack

- **.NET 9** - ASP.NET Core Minimal API
- **SQLite** - local database
- **HTML/CSS/JS** - frontend without frameworks
- **OpenAI Responses API** - quiz generation with web search

### Running the Application

```bash
cd src/InterQuizAI.Api
dotnet run
```

The application will start at `http://localhost:5000`

### Configuration

1. Open the application in your browser
2. Go to **Settings**
3. Set **Master Key** (encryption password, min. 16 characters)
4. Enter **OpenAI API Key**
5. Optionally change the AI model

---

## Screenshots

<img width="1898" height="909" alt="image" src="https://github.com/user-attachments/assets/e31892b8-1bc8-4a8e-82b3-6e81b9bfa9f5" />
<img width="1896" height="817" alt="image" src="https://github.com/user-attachments/assets/102b25f9-338f-4e78-b6ac-be68b0880476" />
<img width="1886" height="899" alt="image" src="https://github.com/user-attachments/assets/87bc9247-04fa-4b31-95c9-409d305e1a37" />
<img width="1889" height="893" alt="image" src="https://github.com/user-attachments/assets/173090b7-2e54-4d6a-bcb2-2f1c52bd69bf" />


---

## License

MIT

---

Made with AI assistance
