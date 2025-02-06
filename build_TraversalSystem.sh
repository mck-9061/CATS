#!/bin/bash
# If you're on Windows, just copy this command and run it in your terminal. Make sure you have everything installed in your venv
pyinstaller TraversalSystem/main.py --onefile --distpath TraversalSystem --workpath TraversalSystem/pyinstaller/build --specpath TraversalSystem/pyinstaller --python-option u
