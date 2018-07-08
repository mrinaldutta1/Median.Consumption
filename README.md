# Median.Consumption
Reads from a input directory a set of files and calculates and outputs the median divergence of the values

Hi There!

You would need to do the following steps to run and execute the solution:
1. Open the .sln file inside MedianConsumption/MedianConsumption in Visual Studio 2017.
2. Change the input folder in the App.Config to your Input Folder which would contain the files to process.
3. Set the log folder in the FileAppender sections of the App.Config. I have set the logging level to INFO.
4. Build and Run the solution to read files from the input directory.

Features:
1. It would look at the input directory and find files which has the file identifiers mentioned in the FileConfiguration.xml
2. For each file, it would run validation steps to check to see if it's a valid file, if not it would log the error and mark the file as having failed validation. The validations which are done at a file level are blank file validation, column validation. The expected column names could be changed in the FileConfiguration.xml file. 
3. Once validation has succeeded it would read the data in the file, for each row it would check if the data in the file is in the appropriate data format, if not it would still continue processing the file, but mark the file as partially processed. 
4. It would then calculate the Median for all data values in the file.
5. Based on the percentage deviation set in the configuration file it would check which rows deviates more or less than the set percentage, then print the values out to the console. This is set to 20 percent by default.
It prints out in this format:
{File Nam} {datetime} {value} {median value}
5. Once all rows have been processed successfully, the file would be marked as successfully processed.
6. The file archival section would determine where to move the file, based on what the file has been marked as. It either goes to the Archive, Error or PartiallyProccesed folders
7. If the logging level is set to INFO it outputs statistics about the files which have been processed.
8. If the logging level is set to ERROR then if the files are malformed or blank it outputs the errors to the logs and console
9. If the logging level is set to FATAL it would only log and display unhandled exceptions.
10. The Columns are shifted around the file would still be processed



Assumptions:
1. Since we are looking at only the data and date/time columns in the input files, I have ignored the other columns in the files. Although I understand probably in a real world scenario something like the UOM columns may need to be taken into consideration to calculate median values for the datasets.




