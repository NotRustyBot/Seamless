# code speed

![flavourful image](https://github.com/NotRustyBot/Seamless/blob/master/docs/CodeSpeed.png?raw=true)

This package can be used to measure the performance of your code.  
It does *not* account for any other performance impact *caused* by your code.    


## usage
To evaluate the performance of your code:


    CodeSpeed.Evaluate((float dt)=>{
        //your code goes here
    });

Behaves like `Events.UpdateCallback`. After a while, the code will stop running and announce the result.  
The resulting score is representing execution time. Score of 100 represents about 1ms.

The raw score is relevant only to your computer. Faster computers will show lower scores. To gauge the performance on different devices, use the adjusted scores.

Adjusted scores are not available until you run a benchmark. To do that:

    CodeSpeed.Benchmark();

The benchmark data will be stored in a file for later use. You don't need to run the benchmark every time you want to preform an evaluation.

## feedback
Feel free to send your feedback to me [Andrej#3024](https://discordapp.com/users/645206726097764364) on Discord.  
If you feel like others might benefit from your feedback, send it to [Map editor](https://discord.gg/jvvZgrb) Discord server.