# code speed



This package can be used to measure the performance of your code.  
It does *not* account for any other performance impact caused by your code.  


## usage
To evaluate the performance of your code:


    CodeSpeed.Evaluate((float dt)=>{
        //your code goes here
    });

Behaves like `Events.UpdateCallback`. After a while, the code will stop running and announce the result.  
The resulting score is representing execution time. Score of 100 represents about 1ms.

The raw score is relevant to your computer. Faster computers will show lower scores. To gauge the performance on different devices, use the adjusted scores.

Adjusted scores are not available until you run a benchmark. To do that:

    CodeSpeed.Benchmark();

If you need

## feedback
Show where users can send feedback about the package.
For example, on [Discord](https://discordapp.com/users/645206726097764364)