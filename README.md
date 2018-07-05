# Median.Consumption
Reads from a input directory a set of files and calculates and outputs the median divergence of the values

Hi There!

You would need to do the following steps to run and execute the solution:
1. Open the .sln file inside MedianConsumption/MedianConsumption in Visual Studio 2017
2. Change the input folder in the App.Config to your Input Folder which would cointain the files to process
3. Build and Run the solution to read the files
4. You may need to upgrade your MSTest.TestAdapter and MSTest.TestFrameWork in case the test cases doesn't work, seems to be known issue
https://stackoverflow.com/questions/42861930/unit-tests-not-discovered-in-visual-studio-2017

Some features of the solution:
1. It would calculate the median Data point for each file, and compare with each row and print the rows which would have divergence 
of more than 20 percent.
2. It would validate the structure of the file to look for columns it is expecting, and output an error if it doesn't find a column 
it is expecting
3. The column names could be changed for future proofing in the FileConfiguration.xml included in the solution, which has the file 
configurations saved.
4. It would check if the data we are looking at has values which are technically not acceptable
5. I have included a couple of basis test cases to test the median values
6. The program also archives the input files into a archive directory, which it creates if not present
7. After processing has been completed it ouputs statistics to the command prompt showing how may files and rows has been processed 
and how many values have deviated from the median more than 20 percent
8. It outputs the unhandled exception to the command window

Assumptions:
1. I have assumed that the other columns in the input files are to be ignored, I understand in a real world scenario that this may not be the case as some rows have different UOMs and any most arithmetic/statistical operations are performed on rows having the same UOM.
As per the requirement in this case I have not taken into consideration these columns.

Future Enhancements/ Possible Areas of Improvement:

1. Include more test cases in the solution
2. Create an error folder along with archive, so in case of exceptions files are moved to the archive location
3. Introduce Interfaces or Dependency Injection to the solution, so as to facilitate ease of testing 
4. Create some amount of logging logic to a .txt file
5. Make the solution more dynamic so that it can dynamically determine the datatype of the target columns


