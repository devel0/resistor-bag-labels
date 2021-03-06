# resistor bag labels

## description

generate dxf suitable to print on A4 adhesive paper for labelling resistor bags.

suggested bag size 12 mm x 8 mm

## quickstart

```sh
git clone https://github.com/devel0/resistor-bag-labels.git
cd resistor-bag-labels
dotnet run
```

this will generate an [output.dxf](output.dxf) you can print preview using [qcad](https://www.qcad.org/en/download) ( sample: [output.pdf](output.pdf) )

## how this project was built

```sh
dotnet new console -n resistor-bag-labels
cd resistor-bag-labels
dotnet add package netDxf.netstandard --version 2.4.0
code .
```