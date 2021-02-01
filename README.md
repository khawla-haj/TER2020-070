
# Software platform for the Internet of Connected Objects and increased assistance to field agents.


In The I3S laboratory, a research teem works on the design and implementation of a software framework to facilitate the continuity of user services in a dynamic way, taking into account the context.

This teem work on the devolopment based on the composition of components and services witch for serve years now represents a potential solution for the construction of large software packages by breaking down features into smaller and, if possible, independent entities that then need to be assembled. And all of these are seen in the LCA (Lightweight Components Architecture) model that is running correctly under WCOMP, a transformation under NODE RED is requested. The aim is to implement the LCA modele into NODE RED.



# Context

Our purpose in this project is to develop and construct a software system to support the continuity of user services by dynamically taking context into account. This platform should also make it possible to recognize the availability and existence of the services and to adapt our application to the development of the infrastructure of the services in order to conform to the reality of the sector.

 The numerous services/devices available are detected using the UPnP protocol. Moreover, each time new UPnP service is discovered by the tool, it will send a command to the WComp container's the tool is connected with, to create a new proxy component corresponding to this new UPnP service. 
   The Weaver enables you to customize and apply rules according to the current context between components. The Weaver has access to a variety of rules in .aa files that are predefined. It will then decide whether the appropriate proxies are present for each of the rules and, if so, apply the corresponding rules by linking a component event to another component method.
