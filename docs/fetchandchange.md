# fetch and change

![flavourful image](https://github.com/NotRustyBot/Seamless/blob/master/docs/FetchAndChange.png?raw=true)

This package makes it easy to select and specific sets of objects, and read parameters of an object, stored in the objects CustomID. This makes it much easier to link objects form the map editor to the script. It also allows to quickly write compatible and reusable code.   


## usage
The package uses IObjects CustomID to define its name, groups and parameters.  
Let's talk terminology:  
- **Name** is a unique string, which should be used on special objects, that are one of a kind.  
- **Group** is a string, that should be given to objects that share some properties. Single object can be a part of multiple groups.  
- **Parameter** is a key-value type string. Not for querying. Single object can have multiple parameters.  

When using these in the map editor, the CustomID might look as follows:
`name#group1#group2(key:"value";price:30)`  
Note that everything is optional. Here are valid examples of some values left out:  
`#coin(value:5;)`  
`finish#interactive`  
`#colorful#interactive`

>**WARNING** do not use the special characters `# ( : ; )` where they shouldn't be.  
>Escape sequence is not available. 


The package provides two static classes: `Fetch` and `Change`.  

### Fetch
The `Fetch` class provides functions to query IObjects either by name or group. It also allows to read the name, groups and parameters of an object.  

>`List<IObject> ByName(string name)`   
>`List<IObject> ByName(string name, IObject[] input)`  
>`List<IObject> ByName(string name, List<IObject> input)`  
Returns an object that has matching name. If `input` is provided, only those objects will be considered.

>`List<IObject> ByGroup(string name)`   
>`List<IObject> ByGroup(string name, IObject[] input)`  
>`List<IObject> ByGroup(string name, List<IObject> input)`  
Returns all objects that are a part of a group. If `input` is provided, only those objects will be considered.

>`string Name(IObject item)`  
Returns the name of the object.

>`string[] Groups(IObject item)`   
Returns the groups that the object is a part of.  

>`Dictionary<string, object> Parameters(string parameters)`   
>`Dictionary<string, object> Parameters(IObject item)`   
Returns key-value dictionary with the objects parameters.


### Change
The `Change` class provides functions to alter name, groups and parameters of the objects.

>`void Name(IObject item, string name)`   
Sets the name of an object.

>`void AddGroup(IObject item, string group)`   
Adds the object to a group.

>`void RemoveGroup(IObject item, string group)`   
Removes object from a group.

>`void Parameter(IObject item, string parameter, object value)`  
Alters the value of a specified parameter, or creates a new one.

>`void RemoveParameter(IObject item, string parameter)`  
Removes the specified parameter from the object.

>`string Object(IObject item, string name, string[] groups, Dictionary<string, object> parameters)`  
>`string Object(string name, string[] groups, Dictionary<string, object> parameters)`  
Sets name, groups and parameters of the object.  
Note that the second overload operates purely with a string, no need for the object to actually exist.

## example
Some objects are marked `#color`, others are `#spin`. And some are `#color#spin`.

        Random rnd = new Random();
        public void OnStartup()
        {
            List<IObject> color = Fetch.ByGroup("color");
            List<IObject> spin = Fetch.ByGroup("spin");

            Events.UpdateCallback.Start((float dt) =>
            {
                foreach (IObject item in color)
                {
                    string[] colors = Game.GetColorPalette(item.GetColorPaletteName()).PrimaryColorPackages;
                    item.SetColor1(colors[rnd.Next(colors.Length)]);
                }
                foreach (IObject item in spin)
                {
                    item.SetAngularVelocity(1);
                }
            },300);
        }

___
A button with CustomID `(heal:30)` will heal 30hp when pressed.

        public void Button(TriggerArgs args) {
            float heal = (float)Fetch.Parameters(args.Caller as IObject)["heal"];
            IPlayer player = args.Sender as IPlayer;
            player.SetHealth(player.GetHealth() + heal);
        }

## performance note
Be advised that these functions are meant as a way of loading information from the map to the script, not as a way to fetch and update it every frame.  
Neither you should use the parameters instead of creating a proper data structure, if it is to be used often. 
## feedback
Feel free to send your feedback to me [Andrej#3024](https://discordapp.com/users/645206726097764364) on Discord.  
If you feel like others might benefit from your feedback, send it to [Map editor](https://discord.gg/jvvZgrb) Discord server.