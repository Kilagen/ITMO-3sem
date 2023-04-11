#!/bin/bash
MODE="+"
num=1

touch pipe5
(tail -n 0 -f pipe5) |
while true
do
        read LINE
        case $LINE in
                \*)
                        MODE="*"
                        echo "mode set to multiplication"
                        ;;
                \+)
                        MODE="+"
                        echo "mode set to addition"
                        ;;
                QUIT)
                        echo "Programm finished"
                        killall tail
                        exit
                        ;;
                *)
                        let res="$num$MODE$LINE"
                        echo "$num$MODE$LINE=$res"
                        num=$res
                        ;;
        esac
done
