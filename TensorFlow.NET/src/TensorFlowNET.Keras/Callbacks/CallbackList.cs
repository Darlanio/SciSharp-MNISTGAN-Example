﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Tensorflow.Keras.Callbacks
{
    public class CallbackList
    {
        List<ICallback> callbacks = new List<ICallback>();
        public History History => callbacks[0] as History;

        public CallbackList(CallbackParams parameters)
        {
            callbacks.Add(new History(parameters));
            callbacks.Add(new ProgbarLogger(parameters));
        }

        public void on_train_begin()
        {
            callbacks.ForEach(x => x.on_train_begin());
        }

        public void on_epoch_begin(int epoch)
        {
            callbacks.ForEach(x => x.on_epoch_begin(epoch));
        }

        public void on_train_batch_begin(long step)
        {
            callbacks.ForEach(x => x.on_train_batch_begin(step));
        }

        public void on_train_batch_end(long end_step, Dictionary<string, float> logs)
        {
            callbacks.ForEach(x => x.on_train_batch_end(end_step, logs));
        }

        public void on_epoch_end(int epoch, Dictionary<string, float> epoch_logs)
        {
            callbacks.ForEach(x => x.on_epoch_end(epoch, epoch_logs));
        }
    }
}
