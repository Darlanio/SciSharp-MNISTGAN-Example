/*****************************************************************************
   Copyright 2018 The TensorFlow.NET Authors. All Rights Reserved.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
******************************************************************************/

using static Tensorflow.Binding;

namespace Tensorflow.Train
{
    public abstract class Trackable
    {
        protected int _self_update_uid;

        /// <summary>
        /// Restore-on-create for a variable be saved with this `Checkpointable`.
        /// </summary>
        /// <returns></returns>
        protected virtual IVariableV1 _add_variable_with_custom_getter(VariableArgs args)
        {
            tf_with(ops.init_scope(), delegate
            {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                IInitializer checkpoint_initializer = null;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
                if (tf.Context.executing_eagerly())
#pragma warning disable CS0642 // Possible mistaken empty statement
                    ;
#pragma warning restore CS0642 // Possible mistaken empty statement
                else
                    checkpoint_initializer = null;
            });

            var new_variable = args.Getter(args);

            // If we set an initializer and the variable processed it, tracking will not
            // assign again. It will add this variable to our dependencies, and if there
            // is a non-trivial restoration queued, it will handle that. This also
            // handles slot variables.
            if (!args.Overwrite || new_variable is RefVariable)
                return _track_checkpointable(new_variable, name: args.Name,
                                        overwrite: args.Overwrite);
            else
                return new_variable;
        }

        /// <summary>
        /// Pop and load any deferred checkpoint restores into `trackable`.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="trackable"></param>
        protected void _handle_deferred_dependencies(string name, IVariableV1 trackable)
        {
            _maybe_initialize_trackable();
            // TODO
        }

        protected IVariableV1 _track_checkpointable(IVariableV1 checkpointable, string name, bool overwrite = false)
        {
            return checkpointable;
        }

        /// <summary>
        /// Initialize dependency management.
        /// </summary>
        protected void _maybe_initialize_trackable()
        {
            // _self_unconditional_checkpoint_dependencies = []
            _self_update_uid = -1;
        }
    }
}
