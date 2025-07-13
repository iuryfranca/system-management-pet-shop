window.allCargos = [];
window.allInscricoesPorCargo = [];
window.donutLabels = [];
window.donutSeries = [];
window.pieLabels = [];
window.pieSeries = [];

window.initDataCharts = function (totalCandidatos, totalCargos, totalInscricoes, cargos, inscricoesPorCargo, donutLabelsParam, donutSeriesParam, pieLabelsParam, pieSeriesParam) {
  console.log('initDataCharts chamado', { totalCandidatos, totalCargos, totalInscricoes, cargos, inscricoesPorCargo });

  window.allCargos = cargos;
  window.allInscricoesPorCargo = inscricoesPorCargo;
  window.donutLabels = donutLabelsParam;
  window.donutSeries = donutSeriesParam;
  window.pieLabels = pieLabelsParam;
  window.pieSeries = pieSeriesParam;

  if (window.chartColumn) { window.chartColumn.destroy(); window.chartColumn = null; }
  if (window.chartDonut) { window.chartDonut.destroy(); window.chartDonut = null; }
  if (window.chartPie) { window.chartPie.destroy(); window.chartPie = null; }

  if (cargos.length === 0 || inscricoesPorCargo.length === 0) {
    return;
  }

  if (typeof ApexCharts !== "undefined" && document.getElementById("column-chart")) {
    const optionsColumnChart = {
      colors: ["#1A56DB"],
      series: [
        {
          name: "Inscrições",
          color: "#1A56DB",
          data: cargos.map((cargo, idx) => ({ x: cargo, y: inscricoesPorCargo[idx] || 0 }))
        }
      ],
      chart: {
        type: "bar",
        height: "320px",
        fontFamily: "Inter, sans-serif",
        toolbar: {
          show: false,
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: "70%",
          borderRadiusApplication: "end",
          borderRadius: 8,
        },
      },
      tooltip: {
        shared: true,
        intersect: false,
        style: {
          fontFamily: "Inter, sans-serif",
        },
      },
      states: {
        hover: {
          filter: {
            type: "darken",
            value: 1,
          },
        },
      },
      stroke: {
        show: true,
        width: 0,
        colors: ["transparent"],
      },
      grid: {
        show: false,
        strokeDashArray: 4,
        padding: {
          left: 2,
          right: 2,
          top: -14,
        },
      },
      dataLabels: {
        enabled: false,
      },
      legend: {
        show: false,
      },
      xaxis: {
        floating: false,
        labels: {
          show: true,
          style: {
            fontFamily: "Inter, sans-serif",
            cssClass: "text-xs font-normal fill-gray-500 dark:fill-gray-400",
          },
        },
        axisBorder: {
          show: false,
        },
        axisTicks: {
          show: false,
        },
      },
      yaxis: {
        show: false,
      },
      fill: {
        opacity: 1,
      },
    }
    window.chartColumn = new ApexCharts(document.getElementById("column-chart"), optionsColumnChart);
    window.chartColumn.render();
  }

  if (typeof ApexCharts !== "undefined" && document.getElementById("donut-chart")) {
    const optionsDonut = {
      series: window.donutSeries,
      labels: window.donutLabels,
      colors: ["#1C64F2", "#16BDCA", "#FDBA8C", "#E74694", "#9061F9", "#F59E42", "#F43F5E"],
      chart: {
        height: 320,
        width: "100%",
        type: "donut",
      },
      stroke: {
        colors: ["transparent"],
        lineCap: "",
      },
      plotOptions: {
        pie: {
          donut: {
            labels: {
              show: true,
              name: {
                show: true,
                fontFamily: "Inter, sans-serif",
                offsetY: 20,
              },
              total: {
                showAlways: true,
                show: true,
                label: "Total Animais",
                fontFamily: "Inter, sans-serif",
                formatter: function (w) {
                  const sum = w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                  return sum;
                },
              },
              value: {
                show: true,
                fontFamily: "Inter, sans-serif",
                offsetY: -20,
                formatter: function (value) {
                  return value;
                },
              },
            },
            size: "80%",
          },
        },
      },
      grid: {
        padding: {
          top: -2,
        },
      },
      dataLabels: {
        enabled: false,
      },
      legend: {
        position: "bottom",
        fontFamily: "Inter, sans-serif",
      },
    };
    window.chartDonut = new ApexCharts(document.getElementById("donut-chart"), optionsDonut);
    window.chartDonut.render();
  }

  if (typeof ApexCharts !== "undefined" && document.getElementById("pie-chart")) {
    const optionsPie = {
      series: window.pieSeries,
      labels: window.pieLabels,
      colors: ["#1C64F2", "#16BDCA", "#9061F9", "#F59E42", "#F43F5E", "#FDBA8C", "#E74694"],
      chart: {
        height: 420,
        width: "100%",
        type: "pie",
      },
      stroke: {
        colors: ["white"],
        lineCap: "",
      },
      plotOptions: {
        pie: {
          labels: {
            show: true,
          },
          size: "100%",
          dataLabels: {
            offset: -25
          }
        },
      },
      dataLabels: {
        enabled: true,
        style: {
          fontFamily: "Inter, sans-serif",
        },
      },
      legend: {
        position: "bottom",
        fontFamily: "Inter, sans-serif",
      },
      yaxis: {
        labels: {
          formatter: function (value) {
            return value;
          },
        },
      },
      xaxis: {
        labels: {
          formatter: function (value) {
            return value;
          },
        },
        axisTicks: {
          show: false,
        },
        axisBorder: {
          show: false,
        },
      },
    };
    window.chartPie = new ApexCharts(document.getElementById("pie-chart"), optionsPie);
    window.chartPie.render();
  }

  const selectCargo = document.getElementById("selectCargoChart");
  if (selectCargo) {
    selectCargo.onchange = function () {
      const idx = selectCargo.value;
      let data;
      if (idx === "") {
        data = window.allCargos.map((cargo, i) => ({ x: cargo, y: window.allInscricoesPorCargo[i] || 0 }));
      } else {
        data = [{ x: window.allCargos[idx], y: window.allInscricoesPorCargo[idx] || 0 }];
      }
      if (window.chartColumn) {
        window.chartColumn.updateSeries([
          {
            name: "Inscrições",
            color: "#1A56DB",
            data: data
          }
        ]);
      }
    }
  }
}; 