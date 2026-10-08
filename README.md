# What's Cookin' – Recipe-Sharing App

A cross-platform recipe-sharing app built with **C# and .NET MAUI** as a group project in the course
*Programming and Design* (SYSB24) at **Lund University**, spring 2025.

Users create an account, post their own recipes, browse everyone's recipes in a shared feed,
and get **AI-generated recipe suggestions** based on what they and the community have posted.

## Features

- **Accounts** – register and log in (passwords stored as SHA-256 hashes), change username and password
- **Profile** – view, edit and delete your own recipes and their ingredients
- **Feed** – browse the latest recipes from all users, with cooking time and cost per portion
- **Add Recipe** – create a recipe with description, instructions, time, cost and ingredients chosen from a shared ingredient list with quantities
- **AI recipe suggestions** – the OpenAI API (gpt-4o-mini) generates recipe ideas in three modes: based on your own recipes, on the newest community recipes, or completely new. Suggestions are shown for inspiration and are not saved as recipes.
- **Ratings and comments** in the data model

## Tech stack

| Area | Tools |
|---|---|
| App | C#, .NET 9, .NET MAUI (XAML) |
| Architecture | MVVM with CommunityToolkit.Mvvm, dependency injection, service layer with interfaces |
| Data | Entity Framework Core, SQL Server (original), SQLite (local fallback) |
| AI | OpenAI API |
| Design | Figma prototypes |

## Team and my contribution

Built by a team of five students. My main contribution was the **Add Recipe** feature: I built the
page and most of its ViewModel logic for creating recipes with ingredients, extended the recipe and
ingredient services it needed, and unified typography and styling across the app's pages.

## Running the app (macOS)

Requirements: .NET 9 SDK, the MAUI workload (`sudo dotnet workload install maui`) and Xcode.

```bash
cd Informatics.MauiSurdeg
cp appsettings.example.json appsettings.json
dotnet build -t:Run -f net9.0-maccatalyst -p:ValidateXcodeVersion=false
```

Without a database connection string the app creates a local SQLite database with demo data on first start.
Demo login: **Pasta_papi** / **password123**.

To enable AI suggestions, add your own OpenAI API key to `appsettings.json` (`AppSettings:OpenAIApiKey`).
`appsettings.json` is git-ignored so keys are never committed.

## Changes after the course

The original app ran against a university SQL Server that no longer exists. To make it runnable again I added:

- a local **SQLite fallback** with automatic table creation and demo data (`DataBase/DatabaseSeeder.cs`)
- a fixed **light theme** and updated input-field colors for readability
- a fix for ingredient lists that only showed one item on macOS

The original database script is kept in `DataBase/WhatsCookin_dll.sql`.
