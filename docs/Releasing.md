# Releasing

To release a new version:

1. Commit changes to https://bitbucket.org/IntelliFactory/websharper.ui.next
2. Build Jenkins "websharper.ui.next" target
3. Build Jenkins "nuget.publisher.private" target to push out the binaries to the private feed
4. Update to the latest NuGet package in example code at https://github.com/intellifactory/websharper.ui.next
5. Verify that the AppVeyor built the new examples and they work online
