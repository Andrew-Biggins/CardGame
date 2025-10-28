# CardGame

## Design Philosophy
- Keep the scoring rules encapsulated behind small, single-purpose services so each concern (input validation, parsing, hand validation, scoring) can evolve independently.
- Use constructor injection to make dependencies explicit and swap them easily in tests; the default parameterless constructors preserve simple wiring for the WPF app.
- Treat the ViewModel as the bridge between UI and engine, exposing only the state the UI needs and keeping it free of UI-specific logic beyond property notifications.

## Testing Philosophy
- Drive each behavior with tests first (TDD) so the public contracts stay stable while refactoring internals.
- Cover each layer with focused unit tests: validators, parser, calculator, engine orchestration, and ViewModel notification behavior.
- Add thin integration tests through the ViewModel to ensure the composed pipeline (parse → validate → score) behaves correctly under real scenarios, including edge cases like Joker handling and invalid input.