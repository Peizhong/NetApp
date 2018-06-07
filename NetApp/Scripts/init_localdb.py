import sqlite3
import os
import random

dbPath = 'mydev.db'

def locateDb():
    if not os.path.exists(dbPath):
        print('set new database')
    else:
        print('already exist')

def setData(length):
    gp = {x:random.randint(1,length) for x in range(length)}
    index0 =1
    index1 = 1  
    conn = sqlite3.connect(dbPath)
    c = conn.cursor()
    c.execute('delete from Topics;')
    c.execute('delete from Entries;')
    for t in gp.items():
        if random.randint(0,t[0]+5) % 2 ==1:
            topic = "主题 "+str(t[0])
        else:
            topic = "Topic "+str(t[0])
        sql = "replace into Topics (id,name,updatetime,ownerid) values(%r, %r, datetime('now'), 1)"%(index0,topic)
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
            sql= "replace into Entries (id,title,text,link,updatetime,topicid) values(%r, %r, %r, %r, datetime('now'), %r);"%(index1,entry,text,"LINK"+str(e),t[0])
            index1+=1
            c.execute(sql)
            print(sql)
    conn.commit()
    conn.close()

if __name__ == '__main__':
    locateDb()
    setData(100)