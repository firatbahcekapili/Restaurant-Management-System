# EmmetNetSharp

EmmetNetSharp: Seamlessly integrate Emmet's HTML/CSS toolkit into your C# development with this powerful NuGet package. Elevate your coding efficiency and streamline web development in C#.

## Installation

To use EmmetNetSharp in your C# project, you need to install the NuGet package. Follow these simple steps:

### Using NuGet Package Manager

1. **Open Your Project**: Open your project in Visual Studio or your preferred IDE.
2. **Open the Package Manager Console**: Navigate to `Tools` -> `NuGet Package Manager` -> `Package Manager Console`.
3. **Install EmmetNetSharp**: Type the following command and press Enter:
   `Install-Package EmmetNetSharp`

### Using .NET CLI

Alternatively, you can use .NET Core CLI to install EmmetNetSharp. Open your command prompt or terminal and run:

`dotnet add package EmmetNetSharp`

### Verifying the Installation

After installation, make sure that EmmetNetSharp is listed in your project dependencies to confirm successful installation.

## Usage

To expand abbreviation, pass it to the `ExpandAbbreviation` method of the `AbbreviationService` class:

```csharp
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
Console.WriteLine(abbreviationService.ExpandAbbreviation("p>a")); // <p><a href=""></a></p>
```

By default, Emmet expands *markup* abbreviation, e.g. abbreviation used for producing nested elements with attributes (like HTML, XML, HAML etc.). If you want to expand *stylesheet* abbreviation, you should pass it as a `Type` property of second argument:

```csharp
using EmmetNetSharp.Enums;
using EmmetNetSharp.Models;
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
var config = new UserConfig
{
    Type = SyntaxType.Stylesheet
};

Console.WriteLine(abbreviationService.ExpandAbbreviation("p10", config)); // padding: 10px
```

A stylesheet abbreviation has slightly different syntax compared to markup one: it doesn’t support nesting and attributes but allows embedded values in element name.

Alternatively, Emmet supports *syntaxes* with predefined snippets and options:

```csharp
using EmmetNetSharp.Enums;
using EmmetNetSharp.Models;
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
var configCss = new UserConfig
{
    Type = SyntaxType.Stylesheet
};

Console.WriteLine(abbreviationService.ExpandAbbreviation("p10", configCss)); // padding: 10px

var configStylus = new UserConfig
{
    Type = "stylus"
};

Console.WriteLine(abbreviationService.ExpandAbbreviation("p10", configStylus)); // padding 10px
```

Predefined syntaxes already have `Type` attribute which describes whether given abbreviation is markup or stylesheet, but if you want to use it with your custom syntax name, you should provide `Type` config option as well (default is `markup`):

```csharp
using EmmetNetSharp.Enums;
using EmmetNetSharp.Models;
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
var config = new UserConfig
{
    Syntax = "my-custom-syntax",
    Type = SyntaxType.Stylesheet,
    Options = new AbbreviationOptions
    {
        StylesheetBetween = "__",
        StylesheetAfter = ""
    }
};

Console.WriteLine(abbreviationService.ExpandAbbreviation("p10", config)); // padding__10px
```

You can pass `Options` property as well to shape-up final output or enable/disable various features. See emmet's config pages for more info and available options.

## Extracting abbreviations from text

A common workflow with Emmet is to type abbreviation somewhere in source code and then expand it with editor action. To support such workflow, abbreviations must be properly _extracted_ from source code:

```csharp
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();

var source = "Hello world ul.tabs>li";
var data = abbreviationService.ExtractAbbreviation(source, 22); // { abbreviation: 'ul.tabs>li' }

Console.WriteLine(abbreviationService.ExpandAbbreviation(data.Abbreviation)); // <ul class="tabs"><li></li></ul>
```

The `ExtractAbbreviation` function accepts source code (most likely, current line) and character location in source from which abbreviation search should be started. The abbreviation is searched in backward direction: the location pointer is moved backward until it finds abbreviation bound. Returned result is an object with `Abbreviation` property and `Start` and `End` properties which describe location of extracted abbreviation in given source.

Most current editors automatically insert closing quote or bracket for `(`, `[` and `{` characters so when user types abbreviation that uses attributes or text, it will end with the following state (`|` is caret location):

```
ul>li[title="Foo|"]
```

E.g. caret location is not at the end of abbreviation and must be moved a few characters ahead. The `ExtractAbbreviation` function is able to handle such cases with `LookAhead` option (enabled by default). This this option enabled, `ExtractAbbreviation` method automatically detects auto-inserted characters and adjusts location, which will be available as `End` property of the returned result:

```csharp
using EmmetNetSharp.Models;
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
var options = new AbbreviationExtractOptions 
{ 
    LookAhead = false 
};

var source = "a div[title] b";
var loc = 11; // right after "title" word

// `lookAhead` is enabled by default
Console.WriteLine(abbreviationService.ExtractAbbreviation(source, loc)); // { abbreviation: 'div[title]', start: 2, end: 12 }
Console.WriteLine(abbreviationService.ExtractAbbreviation(source, loc, options)); // { abbreviation: 'title', start: 6, end: 11 }
```

By default, `ExtractAbbreviation` tries to detect _markup_ abbreviations (see above). _stylesheet_ abbreviations has slightly different syntax so in order to extract abbreviations for stylesheet syntaxes like CSS, you should pass `Type = "stylesheet"` option:

```csharp
using EmmetNetSharp.Enums;
using EmmetNetSharp.Models;
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
var options = new AbbreviationExtractOptions 
{ 
    Type = SyntaxType.Stylesheet 
};

var source = "a{b}";
var loc = 3; // right after "title" word

Console.WriteLine(abbreviationService.ExtractAbbreviation(source, loc)); // { abbreviation: 'a{b}', start: 0, end: 4 }

// Stylesheet abbreviations does not have `{text}` syntax
Console.WriteLine(abbreviationService.ExtractAbbreviation(source, loc, options)); // { abbreviation: 'b', start: 2, end: 3 }
```

### Extract abbreviation with custom prefix

Lots of developers uses React (or similar) library for writing UI code which mixes JS and XML (JSX) in the same source code. Since _any_ Latin word can be used as Emmet abbreviation, writing JSX code with Emmet becomes pain since it will interfere with native editor snippets and distract user with false positive abbreviation matches for variable names, methods etc.:

```js
var div // `div` is a valid abbreviation, Emmet may transform it to `<div></div>`
```

A possible solution for this problem it to use _prefix_ for abbreviation: abbreviation can be successfully extracted only if its preceded with given prefix.

```csharp
using EmmetNetSharp.Models;
using EmmetNetSharp.Services;

var abbreviationService = new AbbreviationService();
var options = new AbbreviationExtractOptions 
{ 
    Prefix = "<" 
};

var source1 = "() => div";
var source2 = "() => <div";

abbreviationService.ExtractAbbreviation(source1, source1.Length); // Finds `div` abbreviation
abbreviationService.ExtractAbbreviation(source2, source2.Length); // Finds `div` abbreviation too

abbreviationService.ExtractAbbreviation(source1, source1.Length, options); // No match, `div` abbreviation is not preceded with `<` prefix
abbreviationService.ExtractAbbreviation(source2, source2.Length, options); // Finds `div` since it preceded with `<` prefix
```

With `prefix` option, you can customize your experience with Emmet in any common syntax (HTML, CSS and so on) if user is distracted too much with Emmet completions for any typed word. A `prefix` may contain multiple character but the last one *must* be a character which is not part of Emmet abbreviation. Good candidates are `<`, `&`, `→` (emoji or Unicode symbol) and so on.

## Other Functionality

EmmetNetSharp also offers other functionality from the Emmet's library, such as HTML and CSS matching, Math expressions and much more.

Take a look at the source code to learn more.

## Support the Project

If you find this project useful, consider supporting it by [buying me a coffee](https://www.buymeacoffee.com/aglasencnik). Your support is greatly appreciated!

## Contributing

Contributions are welcome! If you have a feature to propose or a bug to fix, create a new pull request.

## License

This project is licensed under the [MIT License](https://github.com/aglasencnik/EmmetNetSharp/blob/master/LICENSE.txt).

## Acknowledgment

This project is inspired by and built upon the [Emmet](https://github.com/emmetio) project.
