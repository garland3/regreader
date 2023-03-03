# %%
import pandas as pd

# %%
filename = "../output.csv"
# determine if it exists
import os.path
from os import path
if path.exists(filename):
    print("File exists")
else:
    print("File does not exist")

# %%
# df = pd.read_csv("../output.csv")
# %%
import csv
# Open the CSV file for reading
rows = []
with open(filename, "r") as csvfile:
    # Create a CSV reader object
    csvreader = csv.reader(csvfile)

    # Iterate over each row in the CSV file
    for row in csvreader:
        print(row)
        rows.append(row)
# %%
# count the max number of columns
num_cols =max( [len(a) for a in rows])
print(f"Max number of columns: {num_cols}")
# now create a new list of lists with the correct number of columns

new_rows = []
for row in rows:
    new_row = row + ['']*(num_cols-len(row))
    new_rows.append(new_row)

# %%
# Create a dataframe from the list of lists
# use the first row as the column names PLUS any extra columns
df = pd.DataFrame(new_rows)
df.columns = df.iloc[0]
df = df[1:]
# %%

# df = pd.read_csv(filename,error_bad_lines=False)
# %%
df.head()
# %%
