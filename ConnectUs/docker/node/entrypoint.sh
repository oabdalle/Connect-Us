#!/bin/sh

run_cmd="npm run build"

>&2 echo "Dependencies Installed. Compiling SASS."
exec $run_cmd
