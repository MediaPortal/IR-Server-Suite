2012-05-05 created by Albert
2012-10-12 modified by chefkoch

For the development of MediaPortal 2, we use VisualStudio. Most of our developers use ReSharper (R#) from JetBrains.
That is a tool which provides very useful functions like code formatting, templates, code navigation help and so on.

To have similar settings among the developers, we exported the CodeStyle, CodeInspection and CodeEditing settings to
an external file ("MediaPortal2.DotSettings") and added a reference as an additional layer to each solution.

If a new solution has been created a new layer must be added and referenced to "MediaPortal2.DotSettings".