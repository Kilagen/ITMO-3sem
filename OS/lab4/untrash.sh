#!/bin/bash

CYAN='\033[0;36m'
RED='\033[0;31m'
WHITE='\033[1;37m'
NC='\033[0m'
echo_usage() {
echo -e "${WHITE}Usage:${NC} untrash ${CYAN}filename${NC}"
echo -e "Where ${CYAN}filename${NC} is name of file you want to restore"
echo -e "After calling choose which file you want to restore"
}

echo_err() {
echo -e "${RED}ERR:${NC} $@" 1>&2
}

if [ ! -d "$HOME/.trash" ]
then
        echo_err "$HOME/.trash doesn't exist. Seems that you didn't use rmtrash yet or removed trash folder"
        exit 1
fi

if [ ! -f "$HOME/.trash.log" ]
then
        echo_err "$HOME/.trash.log doesn't exist. Seems that you didn't use rmtrash yet or removed trash log"
        exit 1
fi

if [ "$#" -ne 1 ]
then
        echo_usage
        echo_err "You passed $# arguments instead of 1"
        exit 1
fi

if [[ "$1" =~ "/" ]]
then
        echo_usage
        echo_err "You passed path instead of filename"
        exit 1
fi


lines=$(grep "/$1'$" "$HOME/.trash.log")

if [ -z "$lines" ]
then
        echo "No file named '$1' found"
        exit 0
fi

for line in "$lines"
do
        origin=$(echo "$line" | grep -Po "^'\K.{31}")
        origin="$HOME/.trash/$origin"
        if [ ! -f "$origin" ]
        then
                continue
        fi

        destination=$(echo "$line" | grep -Po "^.{38}\K.*(?='$)")
        folder=$(echo "$destination" | egrep -o "^.*/")
        filename=$(echo "$destination" | grep -Po "^.*/\K.*")
        echo "Folder: $folder"
        echo "File name: $filename"
        echo "Restore '$destination'? y/n"
        read s
        if [ "$s" != y ]
        then
                continue
        fi

        if [ ! -d "$folder" ]
        then
                folder="$HOME/"
                destination="$folder$filename"
                echo "File directory doesn't exist"
                echo "Restoring as '$destination'"
        fi

        msg="Destination file already exists"
        while [ -f "$destination" ]
        do
                echo $msg
                echo "Enter new file name"
                read newname
                if [[ "$newname" =~ "/" ]]
                then
                        msg="Bad file name"
                else
                        destination="$folder$newname"
                        msg="Destination file already exists"
                fi
        done
        echo "Restoring '$origin' to '$destination'"
        ln -- "$origin" "$destination" || {
		echo_err "Hard link failed"
		exit 1
		}
        rm -- "$origin" || {
		echo_err "Can't remove $origin"
		exit 1
		}
done
