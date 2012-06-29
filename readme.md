BugSenseW8
===

BugSense API implementation for Windows 8 Metro-style apps.

Usage
---

```
BugSenseW8.BugSenseHandler.Current.Initialize(<your api key>);
```

It will subscribe to the UnhandledException event, so it won't miss any exceptions. In debug mode, it does not send error reporting to BugSense.

You can explicitly log errors by calling: 

```
BugSenseW8.BugSenseHandler.Current.LogError(<exception object>);
```
