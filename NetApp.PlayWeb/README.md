# Play Gateway

## 准备组件

有那些呢？

## 加载配置

```js
{
    'ReRoutes':[
        {
            'ServiceName': '',
            'DownstreamPathTemplate': '/api/{url}',
            // might load balance
            'DownstreamHostAndPorts':[{
                'host': 'host',
                'port': '8080'
                },
            ]
        }
    ],
    'GlobalConfiguration':{
    }
}
```

## 构建pipeline

传next给下一个

### 缓存

ocelot用的是怎样的?

### 访问目标服务

post, get

## consul

load balance