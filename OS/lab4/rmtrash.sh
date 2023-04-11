#!/bin/bash

WHITE='\033[1;37m'
RED='\033[0;31m'
CYAN='\033[0;36m'
NC='\033[0m'

echo_usage() {
echo -e "${WHITE}Usage: ${NC}rmtrash ${CYAN}file${NC}"
echo -e "Where ${CYAN}file${NC} is file in current working directory that you want to remove"
}

echo_err() {
echo -e "${RED}ERR:${NC} $@" 1>&2
}

if [ $# -ne 1 ]
then
        echo_usage
        echo_err "You passed $# arguments instead of 1"
        exit 1
fi

if [[ "$1" =~ "/" ]]
then
        echo_usage
        echo_err "You passed a filepath instead of a filename"
        exit 1
fi

if [ ! -f "$1" ]
then
        echo_err "No file '${CYAN}$1${NC}' in current directury"
        exit 1
fi

if [ ! -d "$HOME/.trash" ]
then
        mkdir "$HOME/.trash" || {
		echo_err "Can't create dir $HOME/.trash"
		exit 1
		}
fi

linkname="$(date +%FT%T_%N)_$((RANDOM%10))"

ln -- "$1" "$HOME/.trash/$linkname" || {
echo_err "Hard link failed"
exit 1
}

rm -- "$1" || {
echo "Can't remove $1"
exit 1
}

echo "'$linkname' => '$PWD/$1'" >> "$HOME/.trash.log"
