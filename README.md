# Web
~~An attempt to create a~~ Web browser engine implemented in C# from scratch.

## Tech Details
- C# code with minimal syntax sugar (definitely not like [Angular](https://github.com/AngleSharp/AngleSharp))
- Some code is based and inspired by [NetSurf](https://github.com/netsurf-browser/netsurf) wonderful CSS implementation
- [CSQuery](https://github.com/jamietre/CsQuery) is used as HTML parser
- [Skia](https://github.com/google/skia) is used as a graphics library

## Current status
- Able to parse HTML, thanks to CsQuery's HTML parser
- Beginnings of layout engine and CSS parser are being implemented

## Current goal
- To have minimally required code to render some text
