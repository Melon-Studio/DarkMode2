using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace DarkMode_2.ViewModels;

public class SetTimesViewModel : ObservableObject
{

    private IEnumerable<string> _oneComboCollection = new string[] 
    {
        "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"
    };

    private IEnumerable<string> _twoComboCollection = new string[] 
    {
        "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"
    };

    public IEnumerable<string> OneComboCollection
    {
        get => _oneComboCollection;
        set => SetProperty(ref _oneComboCollection, value);
    }

    public IEnumerable<string> TwoComboCollection
    {
        get => _twoComboCollection;
        set => SetProperty(ref _twoComboCollection, value);
    }

}
