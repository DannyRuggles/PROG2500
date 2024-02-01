﻿using System;

namespace CSharp_Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            // data types 
            int num = 10;// integer type

            float f = 3.14f;// float type needs 'f' at the end
            double d = 3.14; // double
            char c = 'c'; // char
            string s = "Hello"; // string
            bool b = true; // bool

            Console.WriteLine("num: " + num + " float: " + f + " double: " + d + " char: " + c + " string: " + s + " bool: " + b);

            // if else example
            if (num > 5)
            {
                Console.WriteLine("num is greater than 5");
            }
            else if (num == 5)
            {
                Console.WriteLine("num is 5");
            }
            else
            {
                Console.WriteLine("num is less than 5");
            }

            // switch example
            switch (c)
            {
                case 'a':
                    Console.WriteLine("c is a");
                    break;
                case 'b':
                    Console.WriteLine("c is b");
                    break;
                default:
                    Console.WriteLine("c is something else");
                    break;
            }

            // operators
            int sum = num + 5; // plus
            int sub = num - 5; // minus
            int mult = num * 2; // multiply
            int div = num / 2; // divide
            int mod = num % 2; // modulus

            Console.WriteLine("sum = " + sum);
            Console.WriteLine("sub = " + sub);
            Console.WriteLine("mult = " + mult);
            Console.WriteLine("div = " + div);
            Console.WriteLine("mod = " + mod);

            // LOOPS
            // for loop
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("for loop: " + i);
            }

            // while loop
            int w = 0;
            while (w < 5)
            {
                Console.WriteLine("while loop: " + w);
                w++;
            }

            // do-while loop
            int dw = 0;
            do
            {
                Console.WriteLine("do-while loop: " + dw);
                dw++;
            } while (dw < 5);

            // Inheritance
            Animal a = new Dog(); // Dog is an Animal
            a.speak(); // should print "Woof!"

            // Error handling
            try
            {
                int zero = 0;
                int result = 10 / zero;
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Can't divide by zero!");
            }
            finally
            {
                Console.WriteLine("This always executes.");
            }
        }
    }

    // base class
    class Animal
    {
        public virtual void speak()
        {
            Console.WriteLine("animal sound");
        }
    }

    // derived class
    class Dog : Animal
    {
        public override void speak()
        {
            // dog says woof
            Console.WriteLine("Woof!");
        }
    }
}

