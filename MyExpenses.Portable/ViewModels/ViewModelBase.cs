//
//  Copyright 2014  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.using System;
using System;
using System.ComponentModel;

namespace MyExpenses.Portable.ViewModels
{
  public class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propName)
    {
      if (PropertyChanged == null)
        return;

      PropertyChanged(this, new PropertyChangedEventArgs(propName));
    }

    private bool isBusy;
    public Action<bool> IsBusyChanged { get; set; }
    public bool IsBusy
    {
      get { return isBusy; }
      set
      {
        isBusy = value;
        OnPropertyChanged("IsBusy");
        if (IsBusyChanged != null)
          IsBusyChanged(isBusy);
      }
    }
    private bool canLoadMore;

    public bool CanLoadMore
    {
      get { return canLoadMore; }
      set
      {
        canLoadMore = value;
        OnPropertyChanged("CanLoadMore");
      }
    }

  }
}
