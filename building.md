# Building
If you want to build this yourself for whatever reason, you'll first need to obtain API keys from Frontier.
Once you've got the keys, put them into MultiCarrierManager/ApiTools/OAuth2.cs.template and remove the .template.
<br><br>
The project can then be built as a regular .NET Framework Windows Forms app.
<br><br>
Once built, you'll need to put the traversal system files with it. In the folder with the built EXE, create a folder called CATS, and copy all the files in the TraversalSystem folder to it.