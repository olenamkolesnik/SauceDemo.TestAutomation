# 🧪 SauceDemo Test Automation Framework

![GitHub Workflow Status](https://github.com/olenamkolesnik/SauceDemo.TestAutomation/actions/workflows/playwright.yml/badge.svg)
[![Allure Report](https://img.shields.io/badge/Allure-Report-blueviolet)](https://github.com/olenamkolesnik/SauceDemo.TestAutomation/actions?query=workflow%3A%22Playwright+Tests%22)

> End-to-end test automation framework powered by .NET 9 + Playwright + NUnit + Allure, integrated with GitHub Actions.

---

## 🚀 Features

- ⚙️ **Playwright** for browser automation
- ✅ **NUnit** for test management
- 📊 **Allure Reporting** with artifacts or GitHub Pages
- 🔁 **CI/CD** with GitHub Actions
- 🔍 **Structured Logging** via `ILogger`

---
## 📁 Project Structure
<pre>
  ├── SauceDemo.TestAutomation/
  │ ├── Pages/ # Page Object Models
  │ ├── Config/ # Configuration & helpers
  │ ├── Hooks/ # Setup / teardown base
  │ └── Tests/ # Actual test cases
  │
  ├── .github/workflows/
  │ └── playwright.yml # CI pipeline
</pre>

---

## 🧪 Running Locally

### 1. Install prerequisites
<pre>
  dotnet tool install --global Microsoft.Playwright.CLI
  playwright install
  npm install -g allure-commandline --unsafe-perm=true
</pre>
### 2. Run the tests
<pre>
  dotnet test
</pre>
### 3. Generate Allure report
<pre>
  allure generate bin/Debug/net9.0/allure-results --clean -o allure-report
  allure open allure-report
</pre>

---

## 🔧 Configuration

Example appsettings.json:
<pre>
  {
    "BaseUrl": "https://www.saucedemo.com",
    "Browser": "chromium",
    "Headless": true,
    "Timeout": 5000
  }
</pre>

---

## 📦 Continuous Integration

GitHub Actions (playwright.yml)
Installs .NET + Playwright + Allure

Runs tests with Allure output

Uploads:

allure-results (raw)

allure-report (HTML)

🔗 [Workflow Runs](https://github.com/olenamkolesnik/SauceDemo.TestAutomation/actions?query=workflow%3A%22Playwright+Tests%22)

---

## 📄 Allure Report Options
### ✅ Option 1: Download via GitHub Actions artifacts
After the workflow finishes:

Go to the GitHub Actions tab → Latest workflow run

Download the allure-report artifact

Run:
<pre>
  allure open <path-to-allure-report>    
</pre>

### 🌐 Option 2: Publish to GitHub Pages (Optional)
To auto-publish Allure reports to Pages:

Add this to your workflow after generating allure-report:

<pre>
  - name: Deploy Allure report to GitHub Pages
    uses: peaceiris/actions-gh-pages@v3
    with:
      github_token: ${{ secrets.GITHUB_TOKEN }}
      publish_dir: allure-report
</pre>

Enable GitHub Pages under Settings → Pages → gh-pages branch.

Then, your live report will be at:

🔗 https://your-org.github.io/your-repo/

---

## ✅ NUnit + Allure Example

<pre>
  [AllureSuite("Login")]
  [AllureSubSuite("Smoke")]
  [AllureSeverity(SeverityLevel.critical)]
  [Test]
  public async Task SuccessfulLogin_ShouldDisplayProductsPage()
  {
      // your test here
  }
</pre>

---

##  Tech Stack

| Tool           | Purpose                |
| -------------- | ---------------------- |
| .NET 9         | Core framework         |
| Playwright     | Browser automation     |
| NUnit          | Test framework         |
| Allure.NUnit   | Reporting integration  |
| GitHub Actions | Continuous integration |
| ILogger        | Structured logging     |

---

## 🧠 Tips

You can use TestContextLogger to redirect logs to both console and test runner

Customize Allure metadata with [AllureSuite], [AllureSeverity], and [AllureTag]

---

## 🙌 Contributions

Found a bug? Want to add a feature?
Feel free to open a pull request or an issue!

---

## 📄 License
MIT License

---
