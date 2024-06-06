out = ""

with open("RAE_Bit_Chart_2.csv", 'r') as csvfile:
    csvlines = csvfile.readlines()
    for line in csvlines:
        split = line.split(',')
        if len(split) == 3:
            channel_name, bit, fixture = split
            out += f"""
[{bit.replace(' ', '')}]
name = "{channel_name.strip()}"
bone = "Root:TODO"
type = "{fixture.strip()}"
""".strip() + '\n\n'
        else:
            out += f"\n# {line}\n"

with open("out.toml", 'w') as outfile:
    outfile.write(out)
