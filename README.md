# Automate Washington Uploads

A tool for inputting CEU completions into the Washington State LNI website

## How it works

- The tradesman completes a CEU course with a provider (eg AnytimeCE)
- The server records the completion and sends an email daily with completions
- This application collects the completion data (either manually or automatically) and accesses the Washington State LNI web application using Selenium
- Each completion is recorded and logged, all errors (misspelled license, out-of-date course) are sent by email to the Headmaster for action
