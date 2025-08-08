# 🧪 SauceDemo Test Automation Framework

[![GitHub Workflow Status](https://github.com/olenamkolesnik/SauceDemo.TestAutomation/actions/workflows/playwright.yml/badge.svg)](https://github.com/olenamkolesnik/SauceDemo.TestAutomation/actions/workflows/playwright.yml)
[![Allure Report](https://img.shields.io/badge/Allure-Report-ff69b4)](https://olenamkolesnik.github.io/SauceDemo.TestAutomation/)

End-to-end test automation framework powered by **.NET 9 + Playwright + NUnit + Allure**, integrated with GitHub Actions and GitHub Pages.

---

## 🚀 Features
- ⚙️ **Playwright** for fast and reliable browser automation
- ✅ **NUnit** for structured test management
- 📊 **Allure Reporting** with screenshots on failure
- 🌐 **Live report publishing** via GitHub Pages
- 🔁 **CI/CD** with GitHub Actions
- 📦 **Test Data Management** using JSON
- 🔍 Structured Logging via `ILogger`

---

## 📁 Project Structure

```
├── SauceDemo.TestAutomation/
│   ├── Pages/         # Page Object Models
│   ├── Models/        # Domain models (e.g., Product)
│   ├── Helpers/       # Test helpers (e.g., TestDataHelper)
│   ├── TestData/      # JSON test data (products, etc.)
│   ├── Hooks/         # Test setup / teardown
│   └── Tests/         # Test cases
│
├── .github/workflows/
│   └── playwright.yml # CI pipeline
```

---

## 🧪 Running Locally

1. **Install prerequisites**
   ```bash
   dotnet tool install --global Microsoft.Playwright.CLI
   playwright install
   npm install -g allure-commandline --unsafe-perm=true
   ```

2. **Run the tests**
   ```bash
   dotnet test
   ```

3. **Generate Allure report**
   ```bash
   allure generate bin/Debug/net9.0/allure-results --clean -o allure-report
   allure open allure-report
   ```

---

## 🔧 Configuration

Example `appsettings.json`:
```json
{
  "BaseUrl": "https://www.saucedemo.com",
  "Browser": "chromium",
  "Headless": true,
  "Timeout": 5000
}
```

---

## 📦 Test Data Management

- Test data is stored in **`TestData/`** as JSON files.  
- Example `products.json`:
```json
[
  {
    "name": "Sauce Labs Bike Light",
    "description": "A red light isn't the desired state in testing...",
    "price": "$9.99"
  }
]
```
- Loaded in tests via:
```csharp
var products = TestDataHelper.LoadProducts();
```

---

## 📸 Screenshots on Failure

- Screenshots are automatically captured when a test fails.
- Added to **Allure report** as attachments.
- Also uploaded as **GitHub Actions artifacts** for download.

---

## 📦 Continuous Integration

The GitHub Actions workflow (`playwright.yml`):
1. Installs **.NET**, **Playwright**, and **Allure**
2. Runs tests **with Allure output**
3. Always uploads:
   - `allure-results` (raw test data)
   - `screenshots` (failures)
4. Builds and publishes the Allure report to GitHub Pages

---

### 🔗 Workflow Runs
- **Artifacts**: Downloadable from the “Artifacts” section in the workflow run.
- **Live Allure Report**:  
  [https://olenamkolesnik.github.io/your-repo/](https://olenamkolesnik.github.io/your-repo/)

---

## ✅ NUnit + Allure Example

```csharp
[AllureSuite("Login")]
[AllureSubSuite("Smoke")]
[AllureSeverity(SeverityLevel.critical)]
[Test]
public async Task SuccessfulLogin_ShouldDisplayProductsPage()
{
    var products = TestDataHelper.LoadProducts();
    // Use products in your test...
}
```

---

## 🧠 Tips
- Use `TestContextLogger` to send logs both to console and test runner
- Customize Allure metadata with `[AllureSuite]`, `[AllureSeverity]`, and `[AllureTag]`
- Keep test data in `TestData/` to avoid hardcoding values in tests

---

## 🙌 Contributions
Found a bug? Want to add a feature?  
Open a **Pull Request** or **Issue** — all contributions are welcome!

---

## 📄 License
MIT License
