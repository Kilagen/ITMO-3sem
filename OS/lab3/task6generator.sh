#!/bin/bash

while true
do
read LINE
        case "$LINE" in
                \+)
                        kill -USR1 $(cat .pid6)
                        ;;
                \*)
                        kill -USR2 $(cat .pid6)
                        ;;
                TERM)
                        kill -SIGTERM $(cat .pid6)
                        exit
                        ;;
        esac
done
