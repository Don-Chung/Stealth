#include <cstdio>
#include <cstdlib>
#include <iostream>
#include <unistd.h>
#include <cstring>
#include <vector>
#include <mysql/mysql.h>
#include <arpa/inet.h>
#include <pthread.h>

using namespace std;

// 信息结构体
struct SockInfo{
    struct sockaddr_in addr;
    int fd;
};
struct SockInfo infos[512];

MYSQL mysql; //mysql连接
MYSQL_FIELD *fd;  //字段列数组
char field[32][32];  //存字段名二维数组
MYSQL_RES *res; //这个结构代表返回行的一个查询结果集
MYSQL_ROW column; //一个行数据的类型安全(type-safe)的表示，表示数据行的列
char query[150]; //查询语句
vector<string> times;
//bool alreadySend = false;

bool ConnectDatabase();
void FreeConnect();
bool QueryTime();
bool InsertTime(int t);
void* working(void* arg);

int main() {
    ConnectDatabase();
    // 1.创建监听的套接字
    int fd = socket(AF_INET, SOCK_STREAM, 0);
    if (fd == -1) {
        perror("socket");
        return -1;
    }

    // 2.绑定本地的IP PORT
    struct sockaddr_in saddr;
    saddr.sin_family = AF_INET;
    saddr.sin_port = htons(9998);
    saddr.sin_addr.s_addr = INADDR_ANY; // 0 = 0.0.0.0
    int ret = bind(fd, (struct sockaddr *) &saddr, sizeof(saddr));
    if (ret == -1) {
        perror("bind");
        return -1;
    }

    // 3.设置监听
    ret = listen(fd, 128);
    if (ret == -1) {
        perror("listen");
        return -1;
    }

    // 初始化结构体数组
    int max = sizeof (infos) / sizeof (infos[0]);
    for(int i = 0; i < max; ++i){
        bzero(&infos[i], sizeof(infos[i]));
        infos[i].fd = -1;
    }

    // 4.阻塞并等待客户端链接
    uint addrlen = sizeof(struct sockaddr_in);
    while(true){
        struct SockInfo* pinfo;
        for (int i = 0; i < max; ++i) {
            if(infos[i].fd == -1){
                pinfo = &infos[i];
                break;
            }
        }
        int cfd = accept(fd, (struct sockaddr *) &pinfo->addr, &addrlen);
        pinfo->fd = cfd;
        if (cfd == -1) {
            perror("accept");
            break;
        }
        // 创建子线程
        pthread_t  tid;
        pthread_create(&tid, NULL, working, pinfo);
        pthread_detach(tid);
    }
    FreeConnect();
    close(fd);

    return 0;
}

void* working(void* arg)
{
    struct SockInfo* pinfo = (struct SockInfo*) arg;
    //
    char ip[32];
    printf("客户端的IP： %s, 端口： %d\n",
           inet_ntop(AF_INET, &pinfo->addr.sin_addr.s_addr, ip, sizeof (ip)),
           ntohs(pinfo->addr.sin_port));

    // 5.通信
    while(true){
        // 接收数据
        char buff[128];
        int len = recv(pinfo->fd, buff, sizeof (buff), 0);
        if(len > 0){
            //数据库操作
            int tmp = atoi(buff);
            InsertTime(tmp);
            printf("client say: %s\n", buff);
            QueryTime();
            string sendTime;
            for(const string& i : times){
                sendTime += ("#" + i);
            }
            printf("%s", sendTime.c_str());
            send(pinfo->fd, sendTime.c_str(), sendTime.size(), 0);
            times.clear();
            sendTime = "";
        }else if(len == 0){
            printf("客户端已经断开了连接...\n");
            break;
        }else{
            perror("recv");
            break;
        }
    }

    //关闭文件描述符
    close(pinfo->fd);
    pinfo->fd = -1;
    return NULL;
}

//连接数据库
bool ConnectDatabase()
{
    //初始化mysql
    mysql_init(&mysql);  //连接mysql，数据库
    const char host[] = "localhost";
    const char user[] = "root";
    const char psw[] = "123456";
    const char table[] = "ranks";
    const int port = 3306;
    //返回false则连接失败，返回true则连接成功
    if (!(mysql_real_connect(&mysql, host, user, psw, table, port, NULL, 0)) )
        //中间分别是主机，用户名，密码，数据库名，端口号（可以写默认0或者3306等），可以先写成参数再传进去
    {
        printf("Error connecting to database:%s\n", mysql_error(&mysql));
        return false;
    }
    else
    {
        printf("Connected...\n");
        return true;
    }
}
//释放资源
void FreeConnect()
{
    mysql_free_result(res);  //释放一个结果集合使用的内存。
    mysql_close(&mysql);     //关闭一个服务器连接。
}

bool InsertTime(int t){
    string query = "insert into times values (NULL, "+ to_string(t) +");";

    if (mysql_query(&mysql, query.c_str()))        //执行SQL语句
    {
        printf("Insert failed (%s)\n", mysql_error(&mysql));
        return false;
    }
    else
    {
        printf("Insert success\n");
        return true;
    }
}

bool QueryTime(){
    strcpy(query, "select t from times order by t");
    if (mysql_query(&mysql, query)){        // 执行指定为一个空结尾的字符串的SQL查询。
        printf("Query failed (%s)\n", mysql_error(&mysql));
        return false;
    }
    else{
        printf("query success\n");
    }
    //获取结果集
    if (!(res = mysql_store_result(&mysql)))    //获得sql语句结束后返回的结果集
    {
        printf("Couldn't get result from %s\n", mysql_error(&mysql));
        return false;
    }

    int rows = mysql_num_rows(res);

    if( rows == 1)
    {
        int cols = mysql_num_fields(res);
        int intTmp = 0;
        MYSQL_ROW row = mysql_fetch_row(res);
        while(intTmp < cols && intTmp < 10)
        {
            times.emplace_back(row[intTmp] == nullptr ? "":row[intTmp]);
            intTmp++;
        }
    }
    else
    {
        int intTmp = 0;
        while(rows-- && intTmp < 10)
        {
            MYSQL_ROW row = mysql_fetch_row(res);
            times.emplace_back(row[0] == nullptr ? "":row[0]);
            intTmp++;
        }
    }
}