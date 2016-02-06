var data;
var start = moment().subtract(1, 'days').format('YYYY-MM-DD');
var end = moment().add(1, 'days').format('YYYY-MM-DD');

var dataUrl = 'http://stationservice.azurewebsites.net/api/stations/1/datapoints/' + start + '/' + end;
console.log(dataUrl);

var colors = ['#377eb8', '#4daf4a', '#1b9e77', '#d95f02', '#7570b3', '#e41a1c'];

d3.json(dataUrl, function (d) {
    data = d;
    //If there is data then build graph
    if (data.length > 0) {
        d3.select("#readingsPlots").select('#loadingNote').text("");
        drawLoop();
    }
});

function drawLoop() {

    // noprotect
    for (i = 0; i < data[0].equipment.length; i++) {
        var station = data[0].Name;
        var id = data[0].equipment[i].ID;
        var sensor = data[0].equipment[i].sensor.Model;
        var readings = data[0].equipment[i].datapoint;
        var type = data[0].equipment[i].sensor.measure.Type;

        plotReadings(type, sensor, id, readings, colors[i]);
    }
}


function plotReadings(measureType, sensorModel, equipmentID, data, color) {

    var type = measureType;

    //Set title for the chart
    var plotTitle = type + " readings from " + sensorModel;
    //Set id for div that will contain a chart
    var readingDivId = "readingDiv" + equipmentID;
    //Create a div for a chart within readingPlots div
    var readingDivs = d3.select("#readingsPlots").append("div")
            .attr("id", readingDivId)
            .attr("class", "plotContainer");
    //console.log(readingDivId);

    var margin = {
        top: 20,
        right: 50,
        bottom: 30,
        left: 50
    },
          width = document.getElementById(readingDivId).offsetWidth - margin.left - margin.right,
          height = 200 - margin.top - margin.bottom;

    readingDivs.append("p")
            .text(plotTitle)
            .attr("class", "title");


    var dataset = data;

    if (data.length === 0) { noDataAvailable(); }

    //custom message if there is no data
    function noDataAvailable() {
        readingDivs.append("p")
            .text("No data available")
            .attr("class", "container");
    }
    console.log(dataset[0].TimeStamp); buildGraph();


    // Build chart
    function buildGraph() {


        var xScale = d3.time.scale()
          .domain([new moment(dataset[0].TimeStamp), new moment(dataset[dataset.length - 1].TimeStamp)])
          .range([0, width]);

        var yScale = d3.scale.linear()
          .domain([0, d3.max(dataset, function (d) {
              return d.Value;
          })])
          .range([height, 0]);
  
        var xAxis = d3.svg.axis()
          .scale(xScale)
          .orient('bottom')
          .ticks(d3.time.hours, 2)
          .tickFormat(d3.time.format('%H:%M'))
          .tickSize(6)
          .tickPadding(8);

        var yAxis = d3.svg.axis()
          .scale(yScale)
          .orient("left")
          .ticks(5);

        var line = d3.svg.line()
          .x(function (d) {
              return xScale(new moment(d.TimeStamp));
          })
          .y(function (d) {
              return yScale(d.Value);
          });

        var svg = d3.select("#" + readingDivId).append("svg")
          .attr("width", width + margin.left + margin.right)
          .attr("height", height + margin.top + margin.bottom)
          .append("g")
          .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        svg.append("g")
          .attr("class", "x axis")
          .attr("transform", "translate(0," + height + ")")
          .call(xAxis);

        svg.append("g")
          .attr("class", "axis")
          .attr("transform", "translate(-5,0)")
          .call(yAxis);

        svg.append("path")
          .data([dataset])
          .attr("class", "line")
          .attr("d", line)
          .style("stroke", color);

        svg.selectAll("circle")
          .data(dataset)
          .enter()
          .append("circle")
          .style("fill", color)
          .attr("class", "marker")
          .attr("cx", function (d) {
              return xScale(new moment(d.TimeStamp));
          })
          .attr("cy", function (d) {
              return yScale(d.Value);
          })
          .attr("r", 5)
          .on("mouseover", function (d) {
              d3.select("#tooltip")
                .style("top", (event.pageY + 15) + "px")
                .style("left", (event.pageX + 15) + "px")
                .select("#value")
                .text(d.Value);
              d3.select("#tooltip")
                 .select("#date")
                .text(moment(d.TimeStamp).format("YYYY-MM-DD HH:mm"));
              d3.select("#tooltip")
                  .select("#readingType")
                  .text(type);
              d3.select("#tooltip").classed("hidden", false);
          })

        .on("mouseout", function () {
            d3.select("#tooltip").classed("hidden", true);
        });

        
    }
}