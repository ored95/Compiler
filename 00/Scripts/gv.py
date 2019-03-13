import os
from graphviz import Source

path = "../Engine/gv"

for file in os.listdir(path):
    if file.endswith(".gv"):
        s = Source.from_file(os.path.join(path, file), format="png")
        s.view()