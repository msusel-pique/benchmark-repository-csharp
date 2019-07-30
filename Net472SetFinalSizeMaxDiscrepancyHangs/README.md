# Net472SetFinalSizeMaxDiscrepancyHangs
Illustrates a bug causing SetFinalSizeMaxDiscrepancy to hang in a loop ... on Net Framework 4.7.2.

Run this project as-is, and you should see a hang, and not see the main window.

Comment out the element noted in MainWindow.xaml and it will run. (Or uncomment the element noted in App.config.)
