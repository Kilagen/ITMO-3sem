#!/bin/bash

echo $$ > .pid6
res=1
MODE="NONE"

usr1() { MODE="+"; }
usr2() { MODE="*"; }
term() { MODE="TERM"; }
trap 'usr1' USR1
trap 'usr2' USR2
trap 'term' SIGTERM
while true
do
        case "$MODE" in
                NONE)
                        ;;
                TERM)
                        echo "Stopped"
                        exit
                        ;;
                \*)
                        let res=$res*2
                        ;;
                \+)
                        let res=$res+2
                        ;;
        esac
        echo $res
        MODE="NONE"
        sleep 1
done
