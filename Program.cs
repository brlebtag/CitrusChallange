using System;
using System.Collections;
using System.Collections.Generic;

namespace CitrusChallange
{
    class Program
    {
        class IntArray {
            private List<dynamic> _intArray;

            public IntArray(string text)
            {
                _intArray = BuildIntArray(text);
            }

            private IntArray(List<dynamic> intArray)
            {
                _intArray = intArray;
            }

            public static IntArray Parse(string text)
            {
                return new IntArray(text);
            }

            public IntArray Flatten()
            {
                var arraysStack = new Stack<List<dynamic>.Enumerator>();
                var flattenArray = new List<dynamic>();

                arraysStack.Push(_intArray.GetEnumerator());

                while(arraysStack.Count > 0)
                {
                    var enumerator = arraysStack.Pop();

                    while (enumerator.MoveNext())
                    {
                        var element = enumerator.Current;
                        
                        if (IsArray(element))
                        {
                            var array = element as List<dynamic>;
                            arraysStack.Push(enumerator);
                            arraysStack.Push(array.GetEnumerator());
                            break;
                        }
                        else
                        {
                            flattenArray.Add(element);
                        }
                    }
                }

                return new IntArray(flattenArray);
            }

            public void Print()
            {
                Print(_intArray);

                Console.WriteLine();
            }

            private void Print(List<dynamic> array)
            {
                var length = array.Count;

                Console.Write("[");

                if (length > 1)
                {
                    Console.Write(array[0]);

                    for(var i = 1; i < length; i++)
                    {
                        var element = array[i];
                        
                        if (IsArray(element))
                        {
                            Console.Write(", ");

                            Print(element);
                        }
                        else
                        {
                            Console.Write($", {element}");
                        }
                    }
                }

                Console.Write("]");
            }

            private bool IsArray(dynamic element)
            {
                return element is IList && element.GetType().IsGenericType;
            }

            private List<dynamic> BuildIntArray(string text)
            {
                var rootList = new List<dynamic>();
                var currentList = rootList;
                var arraysStack = new Stack<List<dynamic>>();
                var element = "";

                foreach(var character in text)
                {
                    if (character == '[')
                    {
                        var newCurrentList = new List<dynamic>();

                        if (currentList != null)
                        {
                            if ( !string.IsNullOrEmpty(element) )
                            {
                                currentList.Add(element);
                            }

                            currentList.Add(newCurrentList);

                            arraysStack.Push(currentList);
                        }

                        element = string.Empty;

                        currentList = newCurrentList;
                    }
                    else if (character >= '0' && character <= '9')
                    {
                        element += character;
                    }
                    else if (character == ',')
                    {
                        if (currentList != null && !string.IsNullOrEmpty(element))
                        {
                            currentList.Add(element);
                        }

                        element = string.Empty;
                    }
                    else if (character == ']')
                    {
                        if ( !string.IsNullOrEmpty(element) )
                        {
                            currentList.Add(element);

                            element = string.Empty;
                        }

                        if (arraysStack.Count > 0)
                        {
                            currentList = arraysStack.Pop();
                        }
                        else
                        {
                            currentList = null;
                        }
                    }
                }

                if (rootList.Count > 0 && IsArray(rootList[0]))
                {
                    return rootList[0];
                }
                else
                {
                    return rootList;
                }
            }
        }

        static void Main(string[] args)
        {
            var input = args;

            if (input.Length <= 1)
            {
                input = ParseStandardInput();
            }

            foreach(var str in input)
            {
                Console.WriteLine("String read");

                Console.WriteLine(str);

                var intArray = new IntArray(str);

                Console.WriteLine("Before Flatten");

                // Before Flatten
                intArray.Print();

                Console.WriteLine("After Flatten");

                // After Flatten
                var newIntArray = intArray.Flatten();
                
                newIntArray.Print();
            }
        }

        static string[] ParseStandardInput()
        {
            var stringList = new List<string>();

            var input = Console.In;

            var line = input.ReadLine();

            while(!string.IsNullOrEmpty(line))
            {
                stringList.Add(line);

                line = input.ReadLine();
            }

            return stringList.ToArray();
        }
    }
}
