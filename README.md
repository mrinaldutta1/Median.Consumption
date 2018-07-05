# Median.Consumption
Reads from a input directory a set of files and calculates and outputs the median divergence of the values

Hi There!

You would need to do the following steps to run and execute the solution:
1. Open the .sln file inside MedianConsumption/MedianConsumption in Visual Studio 2017
2. Change the input folder in the App.Config to you Input Folder which would cointain the files to process
3. Run the solution to read the files
4. You may need to upgrade your MSTest.TestAdapter and MSTest.TestFrameWork in case the test cases doesn't work
https://stackoverflow.com/questions/42861930/unit-tests-not-discovered-in-visual-studio-2017

Some features of the solution:
1. It would calculate the median Data point for each file, and compare with each row and print the rows which would have divergence 
of more than 20 percent.
2. It would validate the structure of the file to look for columns it is expecting, and output an error if it doesn't find a column 
it is expecting
3. The column names could be changed for future proofing in the FileConfiguration.xml included in the solution, which has the file 
configrations saved.
4. It would check if the data we are looking at has values which are technically not acceptable
5. I have included a couple of basis test cases to test the median values
6. The program also archives the input files into a archive directory, which it creates if not present
7. After processing has been completed it ouputs statistics to the command prompt showing how may files and rows has been processed 
and how many values have deviated from the median more than 20 percent
8. It outputs the unhandled exception to the command window
