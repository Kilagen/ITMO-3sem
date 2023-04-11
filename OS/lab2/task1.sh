#!/bin/bash

ps axho user | grep -c "root$" > ans_task1.txt
ps axho pid,comm,user | grep -Po ".*(?=root$)" >> ans_task1.txt
