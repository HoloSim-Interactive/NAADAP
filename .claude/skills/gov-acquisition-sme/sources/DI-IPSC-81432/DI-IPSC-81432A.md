# DI-IPSC-81432A — extracted text

| Field | Value |
| --- | --- |
| Source file | `DI-IPSC-81432_Revision_A_F5BB7EAD93E44C26B5A534F35E9361F9.pdf` |
| Pages | 9 |
| Extraction | OCR (tesseract, 300 dpi, page segmentation mode 4) because ASSIST serves this revision as a scanned image; expect character errors, especially in the DD Form 1664 header boxes and in tables. The PDF is authoritative. |
| Extracted | 2026-09-17 |
| Purpose | Searchable, quotable text for agents and humans. Not a substitute for the PDF when citing. |

---

<!-- page 1 -->

DATA ITEM DESCRIPTION

Title: SYSTEM/SUBSYSTEM DESIGN DESCRIPTION (SSDD)

Number: DI-IPSC-81432A Approval Date: 10 August 1999
AMSC Number: N7351 Limitation: N/A

DTIC Applicable: No GIDEP Applicable: No

Office of Primary Responsibility: N/EC
Applicable Forms: N/A
Use, Relationships:

The System/Subsystem Design Description (SSDD) describes the system- or subsystem-wide
design and the architectural design of a system or subsystem. The SSDD may be supplemented
by Interface Design Descriptions (IDDs) (DI-IPSC-81436A) and Database Design Descriptions
(DBDDs) (DI-IPSC-81437A) as described below.

The SSDD, with it associated IDDs and DBDDs, is used as the basis for further system
development. Throughout this Data Item Description (DID), the term “system” may be
interpreted to mean “subsystem” as applicable. The resulting document should be titled System
Design Description or Subsystem Design Description (SSDD).

This DID contains the format and content preparation instructions for the data product generated
by specific and discrete task requirements as delineated in the contract.

This DID is used when the developer is tasked to define and record the design of a system or
subsystem.

Design pertaining to interfaces may be presented in the SSDD or in IDDs. Design pertaining to
databases may be presented in the SSDD or DBDDs.

The Contract Data Requirements List (CDRL)(DD 1423) will specify whether deliverable data
are to be delivered on paper or electronic media; are to be in a given electronic form (such as
ASCII, CALS, or compatible with a specified word processor or other support software); may be
delivered in developer format rather than in the format specified herein; and may reside ina
computer-aided software engineering (CASE) or other automated tool rather than in the form of
a traditional document.

This DID supersedes DI-IPSC-81432.
Requirements:

1. Reference documents. None.

2. General instructions.

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 2 -->

DI-IPSC-81432A

a. Automated techniques. Use of automated techniques is encouraged. The term
“document” in this DID means a collection of data regardless of its medium.

b. Alternate presentation styles. Diagrams, tables, matrices, and other presentation styles
are acceptable substitutes for text when data required by this DID can be made more readable
using these styles.

3. Format. Following are the format requirements.

a. Title page or identifier. The document shall include a title page containing, as
applicable: document number; volume number; version/revision indicator; security markings or
other restrictions on the handling of the document; date; document title; name, abbreviation, and
any other identifier for the system, subsystem, or item to which the document applies; contract
number; CDRL item number; organization for which the document has been prepared; name and
address of the preparing organization; and distribution statement. For data in a database or other
alternative form, this information shall be included on external and internal labels or by
equivalent identification methods.

b. Table of contents. The document shall contain a table of contents providing the
number, title, and page number of each titled paragraph, figure, table, and appendix. For data in
a database or other alternative form, this information shall consist of an internal or external table
of contents containing pointers to, or instructions for accessing, each paragraph, figure, table, and
appendix or their equivalents.

c. Page numbering/labeling. Each page shall contain a unique page number and display
the document number, including version, volume, and date, as applicable. For data in a database
or other alternative form, files, screens, or other entities shall be assigned names or numbers in
such a way that desired data can be indexed and accessed.

d. Response to tailoring instructions. If a paragraph is tailored out of this DID, the
resulting document shall contain the corresponding paragraph number and title, followed by
“This paragraph has been tailored out.” For data in a database or other alternative form, this
representation need occur only in the table of contents or equivalent.

e. Multiple paragraphs and subparagraphs. Any section, paragraph, or subparagraph in
this DID may be written as multiple paragraphs or subparagraphs to enhance readability.

f. Standard data descriptions. If a data description required by this DID has been
published in a standard data element dictionary specified in the contract, reference to an entry in
that dictionary is preferred over including the description itself.

g. Substitution of existing documents. Commercial or other existing documents may
be substituted for all or part of the document if they contain the required data.

4. Content. The numbers shown designate the paragraph numbers to be used in the document.

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 3 -->

DI-IPSC-81432A

1. Scope. This section shall be divided into the following paragraphs.

1.1 Identification. This paragraph shall contain a full identification of the system and the
software to which this document applies, including, as applicable, identification number(s),
title(s), abbreviation(s), version number(s), and release number(s).

1.2 System overview. This paragraph shall briefly state the purpose of the system and
the software to which this document applies. It shall describe the general nature of the system
and software; summarize the history of system development, operation, and maintenance;
identify the project sponsor, acquirer, user, developer, and support agencies; identify current and
planned operating sites; and list other relevant documents.

1.3 Document overview. This paragraph shall summarize the purpose and contents of
this document and shall describe any security or privacy considerations associated with its use.

2. Referenced documents. This section shall list the number, title, revision, and date of
all documents referenced in this document. This section shall also identify the source for all
documents not available through normal Government stocking activities.

3. System-wide design decisions. This section shall be divided into paragraphs as
needed to present system-wide design decisions, that is, decisions about the system’s behavioral
design (how it will behave, from a user’s point of view, in meeting its requirements, ignoring
internal implementation) and other decisions affecting the selection and design of system
components. If all such decisions are explicit in the requirements or are deferred to the design of
the system components, this section shall so state. Design decisions that respond to requirements
designated critical, such as those for safety, security, or privacy, shall be placed in separate
subparagraphs. Ifa design decision depends upon system states or modes, this dependency shall
be indicated. Design conventions needed to understand the design shall be presented or
referenced. Examples of system-wide design decisions are the following:

a. Design decisions regarding inputs the system will accept and outputs it will produce,
including interfaces with other systems, configuration items, and users (4.3.x of this DID
identifies topics to be considered in this description). If part or all of this information is given in
Interface Design Descriptions (IDDs), they may be referenced.

b. Design decisions on system behavior in response to each input or condition, including
actions the system will perform, response times and other performance characteristics,
description of physical systems modeled, selected equations/algorithms/ rules, and handling of
unallowed inputs or conditions.

c. Design decisions on how system databases/data files will appear to the user (4.3.x of
this DID identifies topics to be considered in this description). If part or all of this information is

given in Database Design Descriptions (DBDDs), they may be referenced.

d. Selected approach to meeting safety, security, and privacy requirements.

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 4 -->

DI-IPSC-81432A

e. Design and construction choices for hardware or hardware-software systems, such as
physical size, color, shape, mass, materials, and markings.

f. Other system-wide design decisions made in response to requirements, such as
selected approach to providing required flexibility, availability, and maintainability.

4. System architectural design. This section shall be divided into the following
paragraphs to describe the system architectural design. If part or all of the design depends upon
system states or modes, this dependency shall be indicated. If design information falls into more
than one paragraph, it may be presented once and referenced from the other paragraphs. Design
conventions needed to understand the design shall be presented or referenced.

Note: For brevity, this section is written in terms of organizing a system directly into
Hardware Configuration Items (HWCIs), Computer Software Configuration Items (CSCIs), and
manual operations, but should be interpreted to cover organizing a system into subsystems,
organizing a subsystem into HWCIs, CSCIs, and manual operations, or other variations as
appropriate.

4.1 System components. This paragraph shall:

a. Identify the components of the system (HWCIs, CSCIs, and manual operations). Each
component shall be assigned a project-unique identifier. Note: a database may be treated as a
CSCI or as part of a CSCI.

b. Show the static (such as “consists of”) relationship(s) of the components. Multiple
relationships may be presented, depending on the selected design methodology.

c. State the purpose of each component and identify the system requirements and
system-wide design decisions allocated to it. (Alternatively, the allocation of requirements may
be provided in 5.a.)

d. Identify each component’s development status/type, if known (such as new
development, existing component to be reused as is, existing design to be reused as is, existing
design or component to be reengineered, component to be developed for reuse, component
planned for Build N, etc.) For existing design or components, the description shall provide
identifying information, such as name, version, documentation references, location, etc.

e. For each computer system or other aggregate of computer hardware resources
identified for use in the system, describe its computer hardware resources (such as processors,
memory, input/output devices, auxiliary storage, and communications/network equipment).
Each description shall, as applicable, identify the configuration items that will use the resource,
describe the allocation of resource utilization to each CSCI that will use the resource (for
example, 20% of the resource’s capacity allocated to CSCI 1, 30% to CSCI 2), describe the
conditions under which utilization will be measured, and describe the characteristics of the
resource:

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 5 -->

DI-IPSC-81432A

1) Descriptions of computer processors shall include, as applicable, manufacturer
name and model number, processor speed/capacity, identification of instruction set architecture,
applicable compiler(s), word size (number of bits in each computer word), character set standard
(such as ASCII, EBCDIC), and interrupt capabilities.

2) Descriptions of memory shall include, as applicable, manufacturer name and
model number and memory size, type, speed, and configuration (such as 256K cache memory,
16MB RAM (4MB x 4)).

3) Descriptions of input/output devices shall include, as applicable, manufacturer
name and model number, type of device, and device speed/capacity.

4) Descriptions of auxiliary storage shall include, as applicable, manufacturer
name and model number, type of storage, amount of installed storage, and storage speed.

5) Descriptions of communications/network equipment, such as modems, network
interface cards, hubs, gateways, cabling, high speed data lines, or aggregates of these or other
components, shall include, as applicable, manufacturer name and model number, data transfer
rates/capacities, network topologies, transmission techniques, and protocols used.

6) Each description shall also include, as applicable, growth capabilities,
diagnostic capabilities, and any additional hardware capabilities relevant to the description.

f. Present a specification tree for the system, that is, a diagram that identifies and shows
the relationships among the planned specifications for the system components.

4.2 Concept of execution. This paragraph shall describe the concept of execution among
the system components. It shall include diagrams and descriptions showing the dynamic
relationship of the components, that is, how they will interact during system assembly, storage,
deployment, and operation, including, as applicable, flow of execution control, data flow,
dynamically controlled sequencing, state transition diagrams, timing diagrams, priorities among
components, handling of interrupts, timing/sequencing relationships, exception handling,
concurrent execution, dynamic allocation/deallocation, dynamic creation/deletion of objects,
processes, tasks, assembly, storage, deployment, and other aspects of dynamic behavior.

4.3 Interface design. This paragraph shall be divided into the following subparagraphs to
describe the interface characteristics of the system components. It shall include both interfaces
among the components and their interfaces with external entities such as other systems,
configuration items, and users. Note: There is no requirement for these interfaces to be
completely designed at this level; this paragraph is provided to allow the recording of interface
design decisions made as part of system architectural design. If part or all of this information is
contained in Interface Design Descriptions (IDDs) or elsewhere, these sources may be
referenced.

4.3.1 Interface identification and diagrams. This paragraph shall state the project-unique
identifier assigned to each interface and shall identify the interfacing entities (systems,

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 6 -->

DI-IPSC-81432A

configuration items, users, etc.) by name, number, version, and documentation references, as
applicable. The identification shall state which entities have fixed interface characteristics (and
therefore impose interface requirements on interfacing entities) and which are being developed
or modified (thus having interface requirements imposed on them). One or more interface
diagrams shall be provided, as appropriate, to depict the interfaces.

4.3.x (Project-unique identifier of interface). This paragraph (beginning with 4.3.2) shall
identify an interface by project-unique identifier, shall briefly identify the interfacing entities,
and shall be divided into subparagraphs as needed to describe the interface characteristics of one
or both of the interfacing entities. If a given interfacing entity is not covered by this SSDD (for
example, an external system) but its interface characteristics need to be mentioned to describe
interfacing entities that are, these characteristics shall be stated as assumptions or as “When [the
entity not covered] does this, [the entity that is covered] will....” This paragraph may reference
other documents (such as data dictionaries, standards for protocols, and standards for user
interfaces) in place of stating the information here. The design description shall include the
following, as applicable, presented in any order suited to the information to be provided, and
shall note any differences in these characteristics from the point of view of the interfacing
entities (such as different expectations about the size, frequency, or other characteristics of data
elements):

a. Priority assigned to the interface by the interfacing entity(ies)

b. Type of interface (such as real-time data transfer, storage-and-retrieval of data, etc.) to
be implemented

c. Characteristics of individual data elements that the interfacing entity(ies) will provide,
store, send, access, receive, etc., such as:

1) Names/identifiers
a) Project-unique identifier
b) Non-technical (natural-language) name
c) DoD standard data element name
d) Technical name (e.g., variable or field name in code or database)
e) Abbreviation or synonymous names
2) Data type (alphanumeric, integer, etc.)
3) Size and format (such as length and punctuation of a character string)

4) Units of measurement (such as meters, dollars, nanoseconds)

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 7 -->

DI-IPSC-81432A

5) Range or enumeration of possible values (such as 0-99)
6) Accuracy (how correct) and precision (number of significant digits)

7) Priority, timing, frequency, volume, sequencing, and other constraints, such as
whether the data element may be updated and whether business rules apply

8) Security and privacy constraints
9) Sources (setting/sending entities) and recipients (using/receiving entities)

d. Characteristics of data element assemblies (records, messages, files, arrays, displays,
reports, etc.) that the interfacing entity (ies) will provide, store, send, access, receive, etc., such
as:

1) Names/identifiers
a) Project-unique identifier
b) Non-technical (natural language) name

c) Technical name (e.g., record or data structure name in code or

database)
d) Abbreviations or synonymous names
2) Data elements in the assembly and their structure (number, order, grouping)
3) Medium (such as disk) and structure of data elements/assemblies on the
medium

4) Visual and auditory characteristics of displays and other outputs (such as
colors, layouts, fonts, icons and other display elements, beeps, lights)

5) Relationships among assemblies, such as sorting/access characteristics

6) Priority, timing, frequency, volume, sequencing, and other constraints, such as
whether the assembly may be updated and whether business rules apply

7) Security and privacy constraints
8) Sources (setting/sending entities) and recipients (using/receiving entities)

e. Characteristics of communication methods that the interfacing entity (ies) will use for
the interface such as:

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 8 -->

DI-IPSC-81432A

1) Project-unique identifier(s)

2) Communication links/bands/frequencies/media and their characteristics

3) Message formatting

4) Flow control (such as sequence numbering and buffer allocation)

5) Data transfer rate, whether periodic/aperiodic, and interval between transfers
6) Routing, addressing, and naming conventions

7) Transmission services, including priority and grade

8) Safety/security/privacy considerations, such as encryption, user authentication,
compartmentalization, and auditing

f. Characteristics of protocols that the interfacing entity(ies) will use for the interface,
such as:

1) Project-unique identifier(s)

2) Priority/layer of the protocol

3) Packeting, including fragmentation and reassembly, routing, and addressing
4) Legality checks, error control, and recovery procedures

5) Synchronization, including connection establishment, maintenance,
termination

_ 6) Status, identification, and any other reporting features

g. Other characteristics, such as physical compatibility of the interfacing entity(ies)
(dimensions, tolerances, loads, voltages, plug compatibility, etc.)

5. Requirements traceability. This section shall contain:

a. Traceability from each system component identified in this SSDD to the system
requirements allocated to it. (Alternatively, this traceability may be provided in 4.1 )

b. Traceability from each system requirement to the system components to which it is
allocated.

6. Notes. This section shall contain any general information that aids in understanding
this document (e.g., background information, glossary, rationale). This section shall include an

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.

---

<!-- page 9 -->

DI-IPSC-81432A

alphabetical listing of all acronyms, abbreviations, and their meanings as used in this document
and a list of any terms and definitions needed to understand this document.

A. Appendices. Appendices may be used to provide information published separately
for convenience in document maintenance (e.g., charts classified data). As applicable, each
appendix shall be referenced in the main body of the document where the data would normally
have been provided. Appendixes may be bound as separate documents for ease in handling.
Appendixes shall be lettered alphabetically (A, B, etc.).

END OF DI-IPSC-81432A

Source: https://assist.dla.mil -- Downloaded: 2026-09-15T20:54Z
Check the source to verify that this is the current version before use.
