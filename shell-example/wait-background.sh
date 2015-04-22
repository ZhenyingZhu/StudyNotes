#! /bin/sh

./background.sh &
PID=$!
echo "now wait background done"
wait $PID
echo "background done!"
