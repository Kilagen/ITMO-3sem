#!/bin/bash

mkdir ~/test &&
        {
        echo "catalog test was created successfully" >> ~/report;
        touch ~/test/$(date +%F_%T)
}

ping "www.net_nikogo.ru" || echo "$(date +%F_%T) ping failed" >> ~/report
