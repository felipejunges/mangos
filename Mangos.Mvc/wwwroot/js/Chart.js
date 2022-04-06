function preencheChart(chartId, labels, dataProjecao, dataRealizacao, valorAnnotation) {
    var ctx = document.getElementById(chartId).getContext('2d');
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'Projeção',
                        data: dataProjecao,
                        backgroundColor: [
                            'rgba(255,255,255,0)'
                        ],
                        borderColor: [
                            'rgba(220,0,220,1)'
                        ],
                        borderWidth: 1
                    },
                    {
                        label: 'Realização',
                        data: dataRealizacao,
                        backgroundColor: [
                            'rgba(0,220,220,0.4)'
                            
                        ],
                        borderColor: [
                            'rgba(0,220,220,1)'
                        ],
                        borderWidth: 2
                    }
                ]
            },
            options: {
                legend: {
                    display: false
                },
                elements: {
                    line: {
                        tension: 0,
                    },
                    point: {
                        radius: 0
                    }
                },
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                },

                annotation: {
                    annotations: [
                        {
                            type: "line",
                            mode: "vertical",
                            scaleID: "x-axis-0",
                            value: valorAnnotation,
                            borderColor: "red",
                            borderDash: [2, 2],
                            //label: {
                            //    content: "TODAY",
                            //    enabled: true,
                            //    position: "top"
                            //}
                        }
                    ]
                }
            }
        });
}

function preencheChart2(chartId, labels, dataProjecao, dataRealizacao, dataRealizacao2, valorAnnotation) {
    var ctx = document.getElementById(chartId).getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Projeção',
                    data: dataProjecao,
                    backgroundColor: [
                        'rgba(255,255,255,0)'
                    ],
                    borderColor: [
                        'rgba(220,0,220,1)'
                    ],
                    borderWidth: 1
                },
                {
                    label: 'Realização',
                    data: dataRealizacao,
                    backgroundColor: [
                        'rgba(0,220,220,0.4)'
                    ],
                    borderColor: [
                        'rgba(0,220,220,1)'
                    ],
                    borderWidth: 2
                },
                {
                    fill: 1,
                    label: 'Realização 2',
                    data: dataRealizacao2,
                    backgroundColor: [
                        'rgba(220,0,220,0.4)'
                    ],
                    borderColor: [
                        'rgba(220,0,220,1)'
                    ],
                    borderWidth: 2
                }
            ]
        },
        options: {
            legend: {
                display: false
            },
            elements: {
                line: {
                    tension: 0,
                },
                point: {
                    radius: 0
                }
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            },

            annotation: {
                annotations: [
                    {
                        type: "line",
                        mode: "vertical",
                        scaleID: "x-axis-0",
                        value: valorAnnotation,
                        borderColor: "red",
                        borderDash: [2, 2],
                        //label: {
                        //    content: "TODAY",
                        //    enabled: true,
                        //    position: "top"
                        //}
                    }
                ]
            }
        }
    });
}