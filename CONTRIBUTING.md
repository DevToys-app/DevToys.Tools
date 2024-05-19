# How to Contribute:

You can contribute to DevToys app by:
- Report issues and bugs [here](https://github.com/DevToys-app/DevToys/issues/new?template=bug_report.md).
- Submit feature requests [here](https://github.com/DevToys-app/DevToys/issues/new?template=feature_request.md).
- Creating a pull request.
- Internationalization and localization:
    * See instructions [here](#internationalization-and-localization).

# How to Build and Run DevToys.Tools from source:

`DevToys.Tools` is an extension for DevToys and DevToys CLI, and follows the same steps than any other regular DevToys extension in order to build and debug it.

## Prerequisites
1. Install [**Visual Studio 2022**](https://visualstudio.microsoft.com/vs/), [**Visual Studio Code**](https://code.visualstudio.com/) with [**C# Dev Kit**](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit), or [**JetBrains Rider**](https://www.jetbrains.com/rider/) depending on your preference.
1. Install [DevToys or DevToys CLI](https://devtoys.app), or [build it from source](https://github.com/DevToys-app/DevToys/blob/main/CONTRIBUTING.md#how-to-build-and-run-devtoys-from-source).

### Finalize your environment
1. Set required environement variables for DevToys following [the official documentation](https://github.com/DevToys-app/Documentation/blob/main/articles/extension-development/getting-started/setup.md).
1. Clone this repository.

### Build, Run & Debug
1. Open `src/DevToys.Tools.sln` with Visual Studio, Visual Studio Code or Rider.
1. Follow the instructions in [the official documentation](https://github.com/DevToys-app/Documentation/blob/main/articles/extension-development/getting-started/debug-an-extension.md#build--run) to build, deploy and debug the extension.

# Internationalization and localization

Please use Crowdin to translate DevToys and its tools. Crowdin is a localization management platform that helps individuals to translate a project without having to be familiar with its repository.

* Go on [DevToy's Crowdin project](https://crowdin.com/project/devtoys).
* Log in or create an account. Join the DevToys project.
* Select the language of your choice in the list of existing supported language and let yourself guided by the website to translate the app.
* If you want to add a new language, please create a new discussion on Crowdin's website or on GitHub. We will be happy to add your language to the list.
* When your translation is done, it will be synchronized with our GitHub repository within 1 hour and create a pull request.
