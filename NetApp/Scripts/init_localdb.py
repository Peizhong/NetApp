import sqlite3
import os
import random

dbPath = 'mydev.db'
initSqls = [
     "CREATE TABLE learning_logs_entry (id integer NOT NULL PRIMARY KEY, title varchar(200) NOT NULL, text text NOT NULL, date_add datetime NOT NULL, topic_id integer NOT NULL REFERENCES learning_logs_topic (id) DEFERRABLE INITIALLY DEFERRED, link varchar(1024) NOT NULL);",
     "CREATE TABLE learning_logs_topic (id integer PRIMARY KEY  NOT NULL ,topic varchar(200) NOT NULL ,date_add datetime NOT NULL ,owner_id INTEGER NOT NULL);"
]

def locateDb():
    if not os.path.exists(dbPath):
        print('set new database')
        conn = sqlite3.connect(dbPath)
        c = conn.cursor()
        (c.execute(sql) for sql in initSqls)
        conn.commit()
        conn.close()
    else:
        print('already exist')

def setData(length):
    gp = {x:random.randint(1,length) for x in range(length)}
    index0 =0
    index1 = 0
    conn = sqlite3.connect(dbPath)
    c = conn.cursor()
    c.execute('delete from learning_logs_topic;')
    c.execute('delete from learning_logs_entry;')
    for t in gp.items():
        if random.randint(0,t[0]+5) % 2 ==1:
            topic = "主题 "+str(t[0])
        else:
            topic = "Topic "+str(t[0])
        sql = "replace into learning_logs_topic (id,topic,date_add,owner_id) values(%r, %r, datetime('now'), 1)"%(index0,topic)
        print(sql)
        c.execute(sql)
        index0+=1
        for e in range(t[1]):
            if random.randint(0,e) % 3 ==1:
                entry = "标题 "+str(e)
            else:
                entry = "Entry "+str(e)
            
            if random.randint(0,e+5) % 2 ==1:
                text = "内容 "+str(e)
            else:
                text = "TEXT "+str(e)
            sql= "replace into learning_logs_entry (id,title,text,link,date_add,topic_id) values(%r, %r, %r, %r, datetime('now'), %r);"%(index1,entry,text,"LINK"+str(e),t[0])
            index1+=1
            c.execute(sql)
            print(sql)
    conn.commit()
    conn.close()

if __name__ == '__main__':
    locateDb()
    setData(100)