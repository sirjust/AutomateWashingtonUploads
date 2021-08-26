# Automate Washington Uploads (AWU)

## Purpose

This program allows a Plumbing and Electrical eLearning company to automatically input course completion data for mandated continuing education into the Washington State L&I Web Application.

## Flow

1. The server collects completion data, and it is input into AWU either automatically or manually
2. The program validates entries for errors (such as mistakenly placing a zero as the second-to-last character)
3. The program uses Selenium to upload the completion data, logging the status
4. Errors, such as an incorrectly spelled license number, are sent by email to support, where they can be resolved manually

## Pre-Requisites

1. The user must create a file called `LoginInfo.cs`, not included in source control, where credentials for the L&I account and the email sender are kept
2. The user must input the data in the following format: `course | date | license | name`, using pipes like so: `WA2016-240 | 2019-03-19 | OOOOOOOOOOOO | LastName, FirstName`
3. As the program uses Chrome, the local Chrome browser should be fully up-to-date.

## Notes

1. As of January 2021, sometimes the ChromeDriver may log an error such as this:

``` cs
[21372:21360:0124/150957.594:ERROR:device_event_log_impl.cc(211)] [15:09:57.595] USB: usb_device_handle_win.cc:1049 Failed to read descriptor from node connection: A device attached to the system is not functioning. (0x1F)
```

This error should be ignored. The Chrome team is working to lower the log event level so this doesn't appear. It has no bearing on the way our `ChromeDriver` operates, and the errors won't appear in our logs or the email the program sends

As of August 2021 these errors should no longer be logging. We can configure the `ChromeDriver` to exclude logging (see `DependencyContainer.cs`)

## Troubleshooting

- At this time, if the email account inside `LoginInfo.cs` is invalid or the device from which the program is running is not recognized, the program will not send the email and will halt when it attempts to write to the log file.
